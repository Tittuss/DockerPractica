<script lang="ts">
    import { api } from '../services/api';
    import { authUser } from '../stores/auth';

    let email = "";
    let password = "";
    let error: string | null = null;
    let loading = false;

    const handleSubmit = async () => {
        error = null;
        loading = true;

        try {
            const response = await api("/auth/login", "POST", {
                email,
                password,
            });

            authUser.set({
                token: response.token,
                nombre: response.nombre,
                rol: response.rol,
            });
        } catch (err) {
            console.error(err);
            error = "Usuario o contraseña incorrectos.";
        } finally {
            loading = false;
        }
    };
</script>

<div class="min-h-screen md:grid md:grid-cols-5">
    <div
        class="hidden md:flex relative items-center justify-center overflow-hidden md:col-span-3 bg-gray-50"
    >
        <svg
            class="absolute inset-0 w-full h-full text-indigo-600"
            viewBox="0 0 815 832"
            preserveAspectRatio="none"
            xmlns="http://www.w3.org/2000/svg"
        >
            <path
                d="M811.855 0C813.934 25.9827 815 52.3365 815 79C815 433.11 627.318 732.662 369 832H0V0H811.855Z"
                fill="currentColor"
            />
        </svg>

        <div class="relative z-10 text-center text-white">
            <div
                class="w-80 h-80 border-4 border-white rounded-full flex items-center justify-center text-4xl font-bold mx-auto"
            >
                IGLESIA APP
            </div>
        </div>
    </div>

    <div
        class="bg-indigo-600 md:bg-white min-h-screen flex flex-col justify-center items-center md:col-span-2"
    >
        <div class="p-4 w-full flex justify-center">
            <div
                class="bg-white p-8 rounded-3xl w-full max-w-sm md:max-w-md shadow-xl md:shadow-none"
            >
                <div class="flex justify-center mb-6 md:hidden">
                    <div
                        class="w-20 h-20 bg-indigo-600 text-white rounded-full flex items-center justify-center font-bold"
                    >
                        App
                    </div>
                </div>

                <h1
                    class="text-3xl lg:text-4xl font-bold text-center text-indigo-700 mb-8"
                >
                    Iniciar sesión
                </h1>

                <form on:submit|preventDefault={handleSubmit} class="space-y-6">
                    <div class="relative">
                        <div
                            class="absolute left-3 top-1/2 -translate-y-1/2 h-5 w-5 text-gray-400"
                        >
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 24 24"
                                stroke-width="1.5"
                                stroke="currentColor"
                                class="w-full h-full"
                            >
                                <path
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                    d="M15.75 6a3.75 3.75 0 11-7.5 0 3.75 3.75 0 017.5 0zM4.501 20.118a7.5 7.5 0 0114.998 0A17.933 17.933 0 0112 21.75c-2.676 0-5.216-.584-7.499-1.632z"
                                />
                            </svg>
                        </div>

                        <input
                            type="text"
                            bind:value={email}
                            placeholder="Email (ej: docente@iglesia.com)"
                            class="w-full pl-12 pr-3 py-3 text-base md:text-lg border rounded-lg border-gray-200 focus:outline-none focus:border-indigo-600 focus:ring-1 focus:ring-indigo-600 transition-colors"
                            required
                        />
                    </div>

                    <div class="relative">
                        <div
                            class="absolute left-3 top-1/2 -translate-y-1/2 h-5 w-5 text-gray-400"
                        >
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                fill="none"
                                viewBox="0 0 24 24"
                                stroke-width="1.5"
                                stroke="currentColor"
                                class="w-full h-full"
                            >
                                <path
                                    stroke-linecap="round"
                                    stroke-linejoin="round"
                                    d="M16.5 10.5V6.75a4.5 4.5 0 10-9 0v3.75m-.75 11.25h10.5a2.25 2.25 0 002.25-2.25v-6.75a2.25 2.25 0 00-2.25-2.25H6.75a2.25 2.25 0 00-2.25 2.25v6.75a2.25 2.25 0 002.25 2.25z"
                                />
                            </svg>
                        </div>

                        <input
                            type="password"
                            bind:value={password}
                            placeholder="Contraseña"
                            class="w-full pl-12 pr-3 py-3 text-base md:text-lg border rounded-lg border-gray-200 focus:outline-none focus:border-indigo-600 focus:ring-1 focus:ring-indigo-600 transition-colors"
                            required
                        />
                    </div>

                    {#if error}
                        <div
                            class="p-3 bg-red-100 border-l-4 border-red-500 text-red-700 text-sm font-medium rounded-md"
                        >
                            <p>{error}</p>
                        </div>
                    {/if}

                    <div
                        class="flex flex-wrap items-center justify-between gap-2 text-sm md:text-base"
                    >
                        <label
                            class="flex items-center gap-2 text-gray-600 cursor-pointer"
                        >
                            <input
                                type="checkbox"
                                class="w-4 h-4 text-indigo-600 border-gray-300 rounded focus:ring-indigo-500"
                            />
                            Recuérdame
                        </label>
                        <a
                            href="/"
                            class="font-medium text-indigo-600 hover:text-indigo-800"
                            >¿Olvidaste tu contraseña?</a
                        >
                    </div>

                    <button
                        type="submit"
                        disabled={loading}
                        class="w-full bg-indigo-600 hover:bg-indigo-700 text-white font-bold py-3 px-4 text-base md:text-lg rounded-lg transition-colors duration-200 disabled:opacity-75 disabled:cursor-not-allowed"
                    >
                        {#if loading}
                            Ingresando...
                        {:else}
                            Iniciar sesión
                        {/if}
                    </button>
                </form>
            </div>
        </div>
    </div>
</div>
