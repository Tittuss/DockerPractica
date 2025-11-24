<script lang="ts">
    import { onMount } from "svelte";
    import { api } from "../services/api";
    import { authUser, logout } from "../stores/auth";
    import type { Curso } from "../types";
    import CourseCard from "./CourseCard.svelte";
    import CourseGradebook from "./CourseGradebook.svelte";

    let courses: Curso[] = [];
    let loading = true;
    let error: string | null = null;
    let selectedCourseId: number | null = null;

    onMount(async () => {
        try {
            courses = await api("/cursos/mis-cursos");
        } catch (err) {
            console.error(err);
            error = "Error al cargar tus cursos asignados.";
        } finally {
            loading = false;
        }
    });

    const handleCourseClick = (id: number) => {
        selectedCourseId = id;
    };
</script>

<div class="min-h-screen bg-gray-50 pb-10">
    <header
        class="bg-white shadow-sm sticky top-0 z-10 border-b border-gray-200"
    >
        <div
            class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4 flex justify-between items-center"
        >
            <div class="flex items-center gap-3">
                <div
                    class="h-10 w-10 bg-gray-800 rounded-lg flex items-center justify-center text-white font-bold text-lg shadow-sm"
                >
                    {$authUser?.nombre.charAt(0)}
                </div>
                <div>
                    <h1 class="text-lg font-bold text-gray-900 leading-tight">
                        Prof. {$authUser?.nombre}
                    </h1>
                    <div class="flex items-center gap-1">
                        <span class="h-2 w-2 rounded-full bg-green-500"></span>
                        <p class="text-xs text-gray-500 font-medium">
                            Panel de Docencia
                        </p>
                    </div>
                </div>
            </div>

            <button
                on:click={logout}
                class="text-sm font-medium text-gray-600 hover:text-red-600 hover:bg-red-50 px-4 py-2 rounded-lg transition-all border border-transparent hover:border-red-100"
            >
                Cerrar Sesión
            </button>
        </div>
    </header>

    <main class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 mt-8">
        {#if selectedCourseId}
            <CourseGradebook
                cursoId={selectedCourseId}
                onBack={() => (selectedCourseId = null)}
            />
        {:else}
            <div class="flex items-end justify-between mb-8">
                <div>
                    <h2 class="text-3xl font-bold text-gray-800 tracking-tight">
                        Mis Cursos
                    </h2>
                    <p class="text-gray-500 mt-1">
                        Gestione sus materias y calificaciones del ciclo actual.
                    </p>
                </div>
                {#if !loading}
                    <span
                        class="bg-gray-800 text-white text-xs font-bold px-3 py-1 rounded-full shadow-sm"
                    >
                        {courses.length} Asignados
                    </span>
                {/if}
            </div>

            {#if loading}
                <div
                    class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6"
                >
                    {#each Array(4) as _}
                        <div
                            class="bg-white h-48 rounded-xl animate-pulse shadow border border-gray-100"
                        ></div>
                    {/each}
                </div>
            {:else if error}
                <div
                    class="bg-red-50 border-l-4 border-red-500 p-6 rounded-r-lg"
                >
                    <div class="flex items-center gap-3">
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            class="h-6 w-6 text-red-600"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke="currentColor"
                        >
                            <path
                                stroke-linecap="round"
                                stroke-linejoin="round"
                                stroke-width="2"
                                d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                            />
                        </svg>
                        <p class="text-red-800 font-medium">{error}</p>
                    </div>
                </div>
            {:else if courses.length === 0}
                <div
                    class="text-center py-20 bg-white rounded-2xl shadow-sm border border-dashed border-gray-300"
                >
                    <div
                        class="bg-gray-50 w-20 h-20 rounded-full flex items-center justify-center mx-auto mb-4"
                    >
                        <svg
                            xmlns="http://www.w3.org/2000/svg"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke-width="1.5"
                            stroke="currentColor"
                            class="w-10 h-10 text-gray-400"
                        >
                            <path
                                stroke-linecap="round"
                                stroke-linejoin="round"
                                d="M12 6.042A8.967 8.967 0 006 3.75c-1.052 0-2.062.18-3 .512v14.25A8.987 8.987 0 016 18c2.305 0 4.408.867 6 2.292m0-14.25a8.966 8.966 0 016-2.292c1.052 0 2.062.18 3 .512v14.25A8.987 8.987 0 0018 18a8.967 8.967 0 00-6 2.292m0-14.25v14.25"
                            />
                        </svg>
                    </div>
                    <h3 class="text-xl font-bold text-gray-900">
                        Sin carga académica
                    </h3>
                    <p class="text-gray-500 mt-2 max-w-sm mx-auto">
                        No tiene cursos asignados para este periodo. Contacte a
                        administración si cree que es un error.
                    </p>
                </div>
            {:else}
                <div
                    class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6"
                >
                    {#each courses as curso (curso.id)}
                        <div
                            role="button"
                            tabindex="0"
                            on:click={() => handleCourseClick(curso.id)}
                            on:keydown={(e) =>
                                e.key === "Enter" &&
                                handleCourseClick(curso.id)}
                            class="transform transition-all duration-200 hover:-translate-y-1 hover:ring-2 hover:ring-indigo-500 hover:ring-offset-2 rounded-xl"
                        >
                            <CourseCard {curso} />
                        </div>
                    {/each}
                </div>
            {/if}
        {/if}
    </main>
</div>
