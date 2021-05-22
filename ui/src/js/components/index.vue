<template>
	<div>
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