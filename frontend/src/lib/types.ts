export interface Curso {
    id: number;
    nombre: string;
    descripcion: string;
    nombreDocente: string;
    promedio?: number;
}

export interface User {
    token: string;
    nombre: string;
    rol: 'Docente' | 'Estudiante' | 'Admin';
}