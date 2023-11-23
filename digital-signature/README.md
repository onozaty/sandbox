# デジタル署名

PKCS#1 v1.5 (RSA) でハッシュ化は SHA256 を使ったデジタル署名＆検証を各言語で書いてみる。

## 秘密鍵/公開鍵作成

RSAで鍵長2048bitの秘密鍵を作成。

```
openssl genrsa -out private.key 2048
```

秘密鍵を元に、公開鍵も作成。

```
openssl rsa -in private.key -pubout -out public.key
```

秘密鍵はPKCS#1なので、PKCS#8のものも用意。

```
openssl pkcs8 -in private.key -topk8 -out private-pkcs8.key -nocrypt
```

## 各言語

標準のライブラリだけ利用して実装する。

### Golang

```golang
package signature

import (
	"crypto"
	"crypto/rsa"
	"crypto/sha256"
	"crypto/x509"
	"encoding/base64"
	"encoding/pem"
	"os"
)

func Sign(target string, privateKey *rsa.PrivateKey) (string, error) {

	signatureBytes, err := rsa.SignPKCS1v15(
		nil,
		privateKey,
		crypto.SHA256,
		sha256hash(target))

	if err != nil {
		return "", err
	}

	return base64.StdEncoding.EncodeToString(signatureBytes), nil
}

func Verify(target string, publicKey *rsa.PublicKey, signature string) (bool, error) {

	signatureBytes, err := base64.StdEncoding.DecodeString(signature)
	if err != nil {
		return false, err
	}

	err = rsa.VerifyPKCS1v15(publicKey, crypto.SHA256, sha256hash(target), signatureBytes)
	return err == nil, nil
}

func ReadPrivateKey(privateKeyPath string) (*rsa.PrivateKey, error) {

	privateKeyBytes, err := os.ReadFile(privateKeyPath)
	if err != nil {
		return nil, err
	}

	privateKeyBlock, _ := pem.Decode(privateKeyBytes)
	privateKey, err := x509.ParsePKCS1PrivateKey(privateKeyBlock.Bytes)
	if err != nil {
		return nil, err
	}
	return privateKey, nil
}

func ReadPublicKey(publicKeyPath string) (*rsa.PublicKey, error) {

	publicKeyBytes, err := os.ReadFile(publicKeyPath)
	if err != nil {
		return nil, err
	}

	publicKeyBlock, _ := pem.Decode(publicKeyBytes)
	publicKey, err := x509.ParsePKIXPublicKey(publicKeyBlock.Bytes)
	if err != nil {
		return nil, err
	}

	return publicKey.(*rsa.PublicKey), nil
}

func sha256hash(target string) []byte {
	hash := sha256.Sum256([]byte(target))
	return hash[:]
}
```

### Java

標準だと PKCS#8 形式しか対応していないので、PKCS#8 でのコードを記載。 

```java
package com.github.onozaty.signature;

import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.security.InvalidKeyException;
import java.security.KeyFactory;
import java.security.NoSuchAlgorithmException;
import java.security.NoSuchProviderException;
import java.security.Signature;
import java.security.SignatureException;
import java.security.interfaces.RSAPrivateKey;
import java.security.interfaces.RSAPublicKey;
import java.security.spec.InvalidKeySpecException;
import java.security.spec.PKCS8EncodedKeySpec;
import java.security.spec.X509EncodedKeySpec;
import java.util.Base64;
import java.util.stream.Collectors;

public class SignatureUtils {

    public static String sign(String target, RSAPrivateKey privateKey)
            throws NoSuchAlgorithmException, InvalidKeyException, SignatureException, NoSuchProviderException {

        Signature signature = Signature.getInstance("SHA256WithRSA");
        signature.initSign(privateKey);
        signature.update(target.getBytes(StandardCharsets.UTF_8));

        byte[] sign = signature.sign();
        return Base64.getEncoder().encodeToString(sign);
    }

    public static boolean verify(String target, RSAPublicKey publicKey, String signatureString)
            throws NoSuchAlgorithmException, InvalidKeyException, SignatureException, NoSuchProviderException {

        Signature signature = Signature.getInstance("SHA256WithRSA");
        signature.initVerify(publicKey);
        signature.update(target.getBytes(StandardCharsets.UTF_8));

        return signature.verify(Base64.getDecoder().decode(signatureString));
    }

    public static RSAPrivateKey readPrivateKey(Path privateKeyFilePath)
            throws IOException, NoSuchAlgorithmException, InvalidKeySpecException {

        // PKCS#8 形式
        String privateKeyString = Files.lines(privateKeyFilePath, StandardCharsets.UTF_8)
                .filter(x -> !x.equals("-----BEGIN PRIVATE KEY-----"))
                .filter(x -> !x.equals("-----END PRIVATE KEY-----"))
                .collect(Collectors.joining());
        byte[] privateKeyBytes = Base64.getDecoder().decode(privateKeyString);

        KeyFactory keyFactory = KeyFactory.getInstance("RSA");
        PKCS8EncodedKeySpec keySpec = new PKCS8EncodedKeySpec(privateKeyBytes);
        return (RSAPrivateKey) keyFactory.generatePrivate(keySpec);
    }

    public static RSAPublicKey readPublicKey(Path publicKeyFilePath)
            throws IOException, NoSuchAlgorithmException, InvalidKeySpecException {

        String publicKeyString = Files.lines(publicKeyFilePath, StandardCharsets.UTF_8)
                .filter(x -> !x.equals("-----BEGIN PUBLIC KEY-----"))
                .filter(x -> !x.equals("-----END PUBLIC KEY-----"))
                .collect(Collectors.joining());
        byte[] publicKeyBytes = Base64.getDecoder().decode(publicKeyString);

        KeyFactory keyFactory = KeyFactory.getInstance("RSA");
        X509EncodedKeySpec keySpec = new X509EncodedKeySpec(publicKeyBytes);
        return (RSAPublicKey) keyFactory.generatePublic(keySpec);
    }
}
```

PKCS#1 形式を利用する際には、Bouncy Castle を使うと良い。

* [The Legion of the Bouncy Castle Java Cryptography APIs](https://www.bouncycastle.org/java.html)

### C#

Javaと同じく、標準だと PKCS#8 形式しか対応していないので、PKCS#8 でのコードを記載。 

```csharp
using System.Security.Cryptography;
using System.Text;

namespace DigitalSignature
{
    public class SignatureUtils
    {
        public static string Sign(string target, string privateKeyPath)
        {
            // PKCS#8 形式
            string privateKeyPEM = string.Join(
                "",
                File.ReadAllLines(privateKeyPath, Encoding.UTF8)
                    .Where(x => x != "-----BEGIN PRIVATE KEY-----" && x != "-----END PRIVATE KEY-----"));

            byte[] privateKeyBytes = Convert.FromBase64String(privateKeyPEM);

            using (var rsa = RSA.Create())
            {
                rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);

                var rsaFormatter = new RSAPKCS1SignatureFormatter(rsa);
                rsaFormatter.SetHashAlgorithm("SHA256");

                byte[] signature = rsaFormatter.CreateSignature(HashData(target));

                return Convert.ToBase64String(signature);
            }
        }
        public static bool Verify(string target, string publicKeyPath, string signature)
        {
            string publicKeyPEM = string.Join(
                "",
                File.ReadAllLines(publicKeyPath, Encoding.UTF8)
                    .Where(x => x != "-----BEGIN PUBLIC KEY-----" && x != "-----END PUBLIC KEY-----"));

            byte[] publicKeyBytes = Convert.FromBase64String(publicKeyPEM);

            using (var rsa = RSA.Create())
            {
                rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

                var rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                rsaDeformatter.SetHashAlgorithm("SHA256");

                return rsaDeformatter.VerifySignature(HashData(target), Convert.FromBase64String(signature));
            }
        }

        private static byte[] HashData(string target)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(target));
            }
        }
    }
}
```

PKCS#1 形式を利用する際には、Bouncy Castle を使うと良い。

* [The Legion of the Bouncy Castle C\# Cryptography APIs](https://www.bouncycastle.org/csharp/index.html)

### Node.js (TypeScript)

```ts
import crypto from 'crypto';

const algorithm = 'RSA-SHA256';
const signatureEncoding: crypto.BinaryToTextEncoding = 'base64';

export const sign = (target: string, privateKey: Buffer): string => {

    const sign = crypto.createSign(algorithm);
    sign.update(target);
    return sign.sign(privateKey, signatureEncoding);
}

export const verify = (target: string, publicKey: Buffer, signature: string): boolean => {

    const verifier = crypto.createVerify(algorithm);
    verifier.update(target);
    return verifier.verify(publicKey, signature, signatureEncoding);
}
```