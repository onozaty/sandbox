package signature

import "testing"

func TestSignVerfy(t *testing.T) {

	privateKeyPath := "../../../private.key"
	publicKeyPath := "../../../public.key"

	target := "Hello"

	privateKey, err := ReadPrivateKey(privateKeyPath)
	if err != nil {
		t.Fatal("failed ReadPrivateKey\n", err)
	}

	signature, err := Sign(target, privateKey)
	if err != nil {
		t.Fatal("failed Sign\n", err)
	}

	if signature != "CPzRkZ82cvMucv6Ph+Eodg1x+afwQF7oL22oMJ1ZrHoHHqusdhN2YzJ+8sTaZmLCPCtadkEEKr/bbI07rHTof4OtD2FGHOYHteng4a8tHZiwuBJLjpDGCPZAlCnl/jpiDA54mR9WZLv4tgAbMPTg9vMqHQVYMJaJAl6e/1RRykunKXAF67iREab6SCxL1Gfjjfax9KHH6GEZnfkjEZ02t0bvI7Qo9cgKAYWe87tOsWYb+RR2gjNHsktp45QG+4eTE17RXnPrY3OY2dBifz1zwDqqEIg1DPrRbSsh04gJXyaB4/MttDOzYJdHVejlacs7iqwRmsysvcMYtVsHeZqIvg==" {
		t.Fatal("failed Sign\n")
	}

	publicKey, err := ReadPublicKey(publicKeyPath)
	if err != nil {
		t.Fatal("failed ReadPublicKey\n", err)
	}

	verified, err := Verify(target, publicKey, signature)
	if err != nil {
		t.Fatal("failed Verify\n", err)
	}

	if !verified {
		t.Fatal("failed Verify\n")
	}
}
