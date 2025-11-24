<script lang="ts">
  import { authUser } from "./lib/stores/auth";

  import Login from "./lib/components/Login.svelte";
  import DocenteDashboard from "./lib/components/DocenteDashboard.svelte";
  import StudentDashboard from "./lib/components/StudentDashboard.svelte";
</script>

<main class="min-h-screen bg-gray-50">
  {#if !$authUser}
    <Login />
  {:else if $authUser.rol === "Docente"}
    <DocenteDashboard />
  {:else}
    <StudentDashboard />
  {/if}
</main>
