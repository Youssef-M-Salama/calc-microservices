const express = require('express');
const axios = require('axios');

const app = express();
const PORT = 3000;

app.use(express.json());

app.get('/', (req, res) => {
    res.status(200).json({
        message: 'API Gateway is running'
    });
});

app.post('/calculate', async (req, res) => {
    const { a, b } = req.body;

    if (typeof a !== 'number' || typeof b !== 'number') {
        return res.status(400).json({ error: 'Invalid input' });
    }

    try {
        const response = await axios.post(
            'http://localhost:4000/calculate',
            { a, b },
            { headers: { 'X-Source': 'api-gateway' } }
        );
        res.status(200).json(response.data);
    } catch (error) {
        res.status(500).json({ error: 'Calculator service unavailable' });
    }
});

app.get('/history', async (req, res) => {
    try {
        const response = await axios.get(
            'http://localhost:5000/history',
            { headers: { 'X-Source': 'api-gateway' } } 
        );
        res.status(200).json(response.data);
    } catch (error) {
        res.status(500).json({ error: 'History service unavailable' });
    }
});

app.listen(PORT, () => {
    console.log(`API Gateway listening on port ${PORT}`);
});