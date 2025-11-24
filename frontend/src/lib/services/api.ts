import { get } from 'svelte/store';
import { authUser } from '../stores/auth';

const API_URL = 'http://localhost:5000/api';

export const api = async (endpoint: string, method: string = 'GET', body: any = null) => {
    const user = get(authUser);

    const headers: HeadersInit = {
        'Content-Type': 'application/json',
    };

    if (user?.token) {
        headers['Authorization'] = `Bearer ${user.token}`;
    }

    const config: RequestInit = {
        method,
        headers,
    };

    if (body) {
        config.body = JSON.stringify(body);
    }

    const response = await fetch(`${API_URL}${endpoint}`, config);

    if (response.status === 401) {
        authUser.set(null);
    }

    if (!response.ok) {
        throw new Error('Error en la petición API');
    }

    return response.json();
};