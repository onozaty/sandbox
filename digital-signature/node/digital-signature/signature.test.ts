import { sign, verify } from './signature';
import fs from 'fs';

const privateKeyPath = './../../private.key';
const publicKeyPath = "./../../public.key";
const expectSignature = 'CPzRkZ82cvMucv6Ph+Eodg1x+afwQF7oL22oMJ1ZrHoHHqusdhN2YzJ+8sTaZmLCPCtadkEEKr/bbI07rHTof4OtD2FGHOYHteng4a8tHZiwuBJLjpDGCPZAlCnl/jpiDA54mR9WZLv4tgAbMPTg9vMqHQVYMJaJAl6e/1RRykunKXAF67iREab6SCxL1Gfjjfax9KHH6GEZnfkjEZ02t0bvI7Qo9cgKAYWe87tOsWYb+RR2gjNHsktp45QG+4eTE17RXnPrY3OY2dBifz1zwDqqEIg1DPrRbSsh04gJXyaB4/MttDOzYJdHVejlacs7iqwRmsysvcMYtVsHeZqIvg==';

test('sign', () => {
    const privateKey = fs.readFileSync(privateKeyPath);
    const target = 'Hello';

    const signature = sign(target, privateKey);
    expect(signature).toBe(expectSignature);
})

test('verify', () => {
    const publicKey = fs.readFileSync(publicKeyPath);
    const target = 'Hello';

    const verified = verify(target, publicKey, expectSignature);
    expect(verified).toBe(true);
})


