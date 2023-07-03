package com.github.onozaty.signature;

import static org.assertj.core.api.Assertions.assertThat;

import java.io.IOException;
import java.nio.file.Path;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.security.NoSuchProviderException;
import java.security.SignatureException;
import java.security.interfaces.RSAPrivateKey;
import java.security.interfaces.RSAPublicKey;
import java.security.spec.InvalidKeySpecException;

import org.junit.jupiter.api.Test;

class SignatureUtilsTest {

    @Test
    void signVerify() throws NoSuchAlgorithmException, InvalidKeySpecException, IOException, InvalidKeyException,
            SignatureException, NoSuchProviderException {

        Path privateKeyPath = Path.of("../../private-pkcs8.key");
        Path publicKeyPath = Path.of("../../public.key");

        String target = "Hello";

        RSAPrivateKey privateKey = SignatureUtils.readPrivateKey(privateKeyPath);
        String signature = SignatureUtils.sign(target, privateKey);

        assertThat(signature)
                .isEqualTo(
                        "CPzRkZ82cvMucv6Ph+Eodg1x+afwQF7oL22oMJ1ZrHoHHqusdhN2YzJ+8sTaZmLCPCtadkEEKr/bbI07rHTof4OtD2FGHOYHteng4a8tHZiwuBJLjpDGCPZAlCnl/jpiDA54mR9WZLv4tgAbMPTg9vMqHQVYMJaJAl6e/1RRykunKXAF67iREab6SCxL1Gfjjfax9KHH6GEZnfkjEZ02t0bvI7Qo9cgKAYWe87tOsWYb+RR2gjNHsktp45QG+4eTE17RXnPrY3OY2dBifz1zwDqqEIg1DPrRbSsh04gJXyaB4/MttDOzYJdHVejlacs7iqwRmsysvcMYtVsHeZqIvg==");

        RSAPublicKey publicKey = SignatureUtils.readPublicKey(publicKeyPath);
        boolean verified = SignatureUtils.verify(target, publicKey, signature);

        assertThat(verified).isTrue();
    }

}
