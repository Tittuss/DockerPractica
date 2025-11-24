<script lang="ts">
    import { onMount } from "svelte";
    import { api } from "../services/api";

    export let cursoId: number;
    export let onBack: () => void; // Función para volver al dashboard

    interface Alumno {
        id: number;
        nombreEstudiante: string;
        nota1: number;
        nota2: number;
        nota3: number;
        notaFinal: number;
    }

    let alumnos: Alumno[] = [];
    let loading = true;
    let savingId: number | null = null;

    onMount(async () => {
        try {
            alumnos = await api(`/cursos/${cursoId}/estudiantes`);
        } catch (error) {
            console.error(error);
            alert("Error al cargar estudiantes");
        } finally {
            loading = false;
        }
    });

    const guardarNotas = async (alumno: Alumno) => {
        savingId = alumno.id;
        try {
            await api(`/inscripciones/${alumno.id}/notas`, "PUT", {
                nota1: Number(alumno.nota1),
                nota2: Number(alumno.nota2),
                nota3: Number(alumno.nota3),
            });
            alumno.notaFinal =
                (Number(alumno.nota1) +
                    Number(alumno.nota2) +
                    Number(alumno.nota3)) /
                3;
            alumnos = alumnos;
            alert(`Notas de ${alumno.nombreEstudiante} guardadas.`);
        } catch (error) {
            console.error(error);
            alert("Error al guardar");
        } finally {
            savingId = null;
        }
    };
</script>

<div class="max-w-7xl mx-auto px-4 mt-8">
    <button
        on:click={onBack}
        class="mb-4 text-indigo-600 hover:underline flex items-center gap-2"
    >
        ← Volver a Mis Cursos
    </button>

    <div
        class="bg-white rounded-xl shadow-lg overflow-hidden border border-gray-100"
    >
        <div
            class="p-6 border-b border-gray-100 flex justify-between items-center"
        >
            <h2 class="text-xl font-bold text-gray-800">
                Libreta de Calificaciones
            </h2>
            <span class="text-sm text-gray-500">Curso ID: {cursoId}</span>
        </div>

        {#if loading}
            <div class="p-10 text-center text-gray-500">
                Cargando lista de estudiantes...
            </div>
        {:else if alumnos.length === 0}
            <div class="p-10 text-center text-gray-500">
                No hay estudiantes inscritos en este curso.
            </div>
        {:else}
            <div class="overflow-x-auto">
                <table class="w-full text-left border-collapse">
                    <thead>
                        <tr
                            class="bg-gray-50 text-gray-600 text-sm uppercase tracking-wider"
                        >
                            <th class="p-4 border-b">Estudiante</th>
                            <th class="p-4 border-b w-24">Nota 1</th>
                            <th class="p-4 border-b w-24">Nota 2</th>
                            <th class="p-4 border-b w-24">Nota 3</th>
                            <th class="p-4 border-b w-24">Promedio</th>
                            <th class="p-4 border-b w-32">Acción</th>
                        </tr>
                    </thead>
                    <tbody>
                        {#each alumnos as alumno (alumno.id)}
                            <tr
                                class="hover:bg-gray-50 transition-colors border-b last:border-0"
                            >
                                <td class="p-4 font-medium text-gray-900">
                                    {alumno.nombreEstudiante}
                                </td>
                                <td class="p-4">
                                    <input
                                        type="number"
                                        min="0"
                                        max="100"
                                        class="w-full border rounded px-2 py-1 focus:ring-2 ring-indigo-500 outline-none"
                                        bind:value={alumno.nota1}
                                    />
                                </td>
                                <td class="p-4">
                                    <input
                                        type="number"
                                        min="0"
                                        max="100"
                                        class="w-full border rounded px-2 py-1 focus:ring-2 ring-indigo-500 outline-none"
                                        bind:value={alumno.nota2}
                                    />
                                </td>
                                <td class="p-4">
                                    <input
                                        type="number"
                                        min="0"
                                        max="100"
                                        class="w-full border rounded px-2 py-1 focus:ring-2 ring-indigo-500 outline-none"
                                        bind:value={alumno.nota3}
                                    />
                                </td>
                                <td class="p-4 font-bold text-indigo-600">
                                    {alumno.notaFinal.toFixed(1)}
                                </td>
                                <td class="p-4">
                                    <button
                                        on:click={() => guardarNotas(alumno)}
                                        disabled={savingId === alumno.id}
                                        class="bg-indigo-600 hover:bg-indigo-700 text-white text-xs px-3 py-2 rounded transition-colors disabled:opacity-50"
                                    >
                                        {savingId === alumno.id
                                            ? "..."
                                            : "Guardar"}
                                    </button>
                                </td>
                            </tr>
                        {/each}
                    </tbody>
                </table>
            </div>
        {/if}
    </div>
</div>
