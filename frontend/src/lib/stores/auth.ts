import { writable } from 'svelte/store';

const isBrowser = typeof window !== 'undefined';

interface User {
    token: string;
    nombre: string;
    rol: 'Docente' | 'Estudiante';
}

const storedUser = isBrowser ? localStorage.getItem('usuario_iglesia') : null;
const initialUser: User | null = storedUser ? JSON.parse(storedUser) : null;

export const authUser = writable<User | null>(initialUser);

authUser.subscribe((value) => {
    if (isBrowser) {
        if (value) {
            localStorage.setItem('usuario_iglesia', JSON.stringify(value));
        } else {
            localStorage.removeItem('usuario_iglesia');
        }
    }
});

export const logout = () => {
    authUser.set(null);
    if (isBrowser) window.location.href = '/login';
};