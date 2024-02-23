let apiUrl;

if (process.env.NODE_ENV === 'development') {
    apiUrl = 'localhost:5000';
} else {
    apiUrl = 'haijiuvpn.ddns.net:5000';
}

export const API_URL = apiUrl;