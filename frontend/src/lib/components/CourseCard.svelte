<script lang="ts">
    import type { Curso } from "../types";

    export let curso: Curso;

    const colors = [
        "bg-blue-500",
        "bg-green-500",
        "bg-purple-500",
        "bg-orange-500",
    ];
    const randomColor = colors[curso.id % colors.length]; // Color consistente basado en ID
</script>

<div
    class="bg-white rounded-xl shadow-sm hover:shadow-md transition-shadow duration-200 overflow-hidden flex border border-gray-100 group cursor-pointer"
>
    <div
        class={`w-2 ${randomColor} group-hover:w-3 transition-all duration-200`}
    ></div>

    <div class="p-5 flex-1">
        <h3
            class="text-lg font-bold text-gray-800 mb-1 group-hover:text-indigo-600 transition-colors"
        >
            {curso.nombre}
        </h3>

        <div class="flex items-center gap-2 text-sm text-gray-500 mb-3">
            <svg
                xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 20 20"
                fill="currentColor"
                class="w-4 h-4"
            >
                <path
                    d="M10 8a3 3 0 100-6 3 3 0 000 6zM3.465 14.493a1.25 1.25 0 00.41 1.412A9.957 9.957 0 0010 18c2.31 0 4.438-.784 6.131-2.1.43-.333.604-.903.408-1.41a7.002 7.002 0 00-13.074.003z"
                />
            </svg>
            <span>{curso.nombreDocente}</span>
        </div>

        {#if curso.promedio !== undefined && curso.promedio !== null}
            <div class="mb-3">
                <span class="text-xs uppercase font-bold text-gray-500"
                    >Promedio:</span
                >
                <span
                    class={`font-bold text-lg ${curso.promedio >= 70 ? "text-green-600" : "text-orange-500"}`}
                >
                    {curso.promedio.toFixed(1)}
                </span>
            </div>
        {/if}

        <p class="text-gray-600 text-sm line-clamp-2">
            {curso.descripcion}
        </p>

        <div class="mt-4 flex justify-end">
            <span
                class="text-xs font-medium px-2.5 py-0.5 rounded bg-gray-100 text-gray-600"
            >
                Presencial
            </span>
        </div>
    </div>
</div>
