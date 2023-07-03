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
