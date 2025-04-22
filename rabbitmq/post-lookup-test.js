import http from 'k6/http';
import { check } from 'k6';

export const options = {
    vus: 6, // Number of virtual users
    duration: '1000s', // Duration of the test
};

export default function () {
    const url = 'http://localhost:5173/lookup'; // Replace with actual URL if different

    const payload = JSON.stringify({
        id: "00000000-0000-0000-0000-000000000001",
        continuumOrderIdentifier: "CONT-ABC123",
        merchantOrderIdentifier: "MER-123",
        merchantId: 101,
        saleCurrencyId: 978,
        saleValue: 123.45,
        resultCodeId: 0,
        creationTimestamp: "2025-04-18T12:00:00Z",
        versionSequence: 1,
        orderSessionId: "SESSION-999"
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    const res = http.post(url, payload, params);

    check(res, {
        'status was 201': (r) => r.status === 201,
    });
}
