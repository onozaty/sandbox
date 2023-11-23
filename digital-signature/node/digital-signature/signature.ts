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
