# Plataforma de Gestión Académica (Iglesia App)

## Tecnologías

* **Frontend:** Svelte (Vite) + Tailwind CSS v4 + TypeScript.
* **Backend:** ASP.NET Core Web API (.NET 8).
* **Base de Datos:** Microsoft SQL Server 2022 (Dockerizado).
* **Infraestructura:** Docker Compose.

## Prerrequisitos

* Docker Desktop

## Instalación y Ejecución
1.  **Clonar el repositorio:**
    ```bash
    git clone https://github.com/Tittuss/DockerPractica.git
    cd DockerPractica
    ```

2.  **Construir y levantar los contenedores:**
    Ejecuta el siguiente comando en la raíz del proyecto (donde está el archivo `docker-compose.yml`):
    ```bash
    docker-compose up --build
    ```
    *Esperar unos minutos la primera vez mientras se descargan las imágenes y se compila el proyecto.*

3.  **Verificar estado:**
    En la consola un mensaje indicando que la base de datos se ha creado exitosamente:
    `--> ¡Base de datos creada/verificada exitosamente!`

## Inicialización de Datos (Seed)

La base de datos se crea vacía. Para poblarla con usuarios y cursos de prueba, ejecutar el endpoint de "Seed".

1.  Abrir navegador o Postman.
2.  Visitar la siguiente URL:
    [http://localhost:5000/api/seed](http://localhost:5000/api/seed)
3.  Mensaje de exito:
    `{"message":"Datos de prueba creados exitosamente..."}`

## Credenciales de Acceso

Usuarios de prueba:

### Rol Docente
* **Email:** `docente@iglesia.com`
* **Contraseña:** `123`
* **Funciones:** Ver cursos asignados, ver lista de alumnos, calificar estudiantes.

### Rol Estudiante
* **Email:** `alumno@iglesia.com`
* **Contraseña:** `123`
* **Funciones:** Ver cursos inscritos, ver promedio y calificaciones.

## Acceso a la Aplicación

* **Frontend (Web):** [http://localhost:3000](http://localhost:3000)
* **Backend (Swagger/API):** [http://localhost:5000/api/...](http://localhost:5000)
