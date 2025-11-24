<script lang="ts">
    import { onMount } from "svelte";
    import { api } from "../services/api";
    import { authUser, logout } from "../stores/auth";
    import type { Curso } from "../types";
    import CourseCard from "./CourseCard.svelte";

    let courses: Curso[] = [];
    let loading = true;
    let error: string | null = null;

    onMount(async () => {
        try {
            courses = await api("/cursos/mis-cursos");
        } catch (err) {
            console.error(err);
            error = "No se pudieron cargar tus materias. Intenta nuevamente.";
        } finally {
            loading = false;
        }
    });
</script>

<div class="min-h-screen bg-gray-50 pb-10">
    <header class="bg-white shadow-sm sticky top-0 z-10">
        <div
            class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4 flex justify-between items-center"
        >
            <div class="flex items-center gap-3">
                <div
                    class="h-8 w-8 bg-indigo-600 rounded-full flex items-center justify-center text-white font-bold text-sm"
                >
                    {$authUser?.nombre.charAt(0)}
                </div>
                <div>
                    <h1 class="text-lg font-bold text-gray-900 leading-tight">
                        Hola, {$authUser?.nombre}
                    </h1>
                    <p class="text-xs text-gray-500">Estudiante</p>
                </div>
            </div>

            <button
                on:click={logout}
                class="text-sm font-medium text-red-600 hover:text-red-800 hover:bg-red-50 px-3 py-2 rounded-lg transition-colors"
            >
                Cerrar Sesión
            </button>
        </div>
    </header>

    <main class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
        <section class="mb-10">
            <h2 class="text-2xl font-bold text-gray-800 mb-4">
                Noticias y Anuncios
            </h2>
            <div
                class="bg-gradient-to-r from-indigo-500 to-purple-600 rounded-2xl p-6 text-white shadow-lg relative overflow-hidden"
            >
                <div class="relative z-10">
                    <h3 class="text-xl font-bold mb-2">
                        ¡Bienvenido al nuevo ciclo!
                    </h3>
                    <p class="opacity-90 max-w-2xl">
                        Estamos emocionados de verte de nuevo. Recuerda revisar
                        tus horarios y estar atento a las notificaciones de tus
                        docentes.
                    </p>
                </div>
                <div
                    class="absolute right-0 top-0 h-full w-1/3 bg-white opacity-10 transform skew-x-12"
                ></div>
            </div>
        </section>

        <section>
            <h2
                class="text-2xl font-bold text-gray-800 mb-6 flex items-center gap-2"
            >
                Mis Materias
                {#if !loading && courses.length > 0}
                    <span
                        class="bg-gray-200 text-gray-700 text-xs py-1 px-2 rounded-full"
                        >{courses.length}</span
                    >
                {/if}
            </h2>

            {#if loading}
                <div
                    class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6"
                >
                    {#each Array(3) as _}
                        <div
                            class="bg-white h-40 rounded-xl animate-pulse shadow-sm"
                        ></div>
                    {/each}
                </div>
            {:else if error}
                <div
                    class="bg-red-50 border-l-4 border-red-500 p-4 rounded text-red-700"
                >
                    <p class="font-bold">Error</p>
                    <p>{error}</p>
                </div>
            {:else if courses.length === 0}
                <div
                    class="bg-white p-12 rounded-xl shadow-sm text-center border border-dashed border-gray-300"
                >
                    <svg
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        stroke-width="1.5"
                        stroke="currentColor"
                        class="w-16 h-16 mx-auto text-gray-400 mb-4"
                    >
                        <path
                            stroke-linecap="round"
                            stroke-linejoin="round"
                            d="M12 6.042A8.967 8.967 0 006 3.75c-1.052 0-2.062.18-3 .512v14.25A8.987 8.987 0 016 18c2.305 0 4.408.867 6 2.292m0-14.25a8.966 8.966 0 016-2.292c1.052 0 2.062.18 3 .512v14.25A8.987 8.987 0 0018 18a8.967 8.967 0 00-6 2.292m0-14.25v14.25"
                        />
                    </svg>
                    <h3 class="text-lg font-medium text-gray-900">
                        No tienes materias inscritas
                    </h3>
                    <p class="text-gray-500 mt-1">
                        Parece que no estás inscrito en ningún curso para este
                        ciclo.
                    </p>
                </div>
            {:else}
                <div
                    class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6"
                >
                    {#each courses as curso (curso.id)}
                        <CourseCard {curso} />
                    {/each}
                </div>
            {/if}
        </section>
    </main>
</div>
