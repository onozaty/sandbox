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
