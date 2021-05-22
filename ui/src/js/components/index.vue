<template>
	<div class="main">
		<spa-link target="/test">Test!</spa-link>
	</div>
</template>

<script lang="ts">
import Vue from 'vue';
import SpaLink from '@/SPA/SpaLink.vue';

export default Vue.extend({
	components: {
		"spa-link": SpaLink
	}
})
</script>

<style lang="scss" scoped>
	.main {
		width: 100vw;
		min-height: 100vh;
		background-color: #444;

		a {
			text-decoration: none;
			color: rgb(64, 128, 255);
		}

		a:visited {
			color: rgb(48, 80, 160);
			text-decoration: none;
		}
	}
</style>