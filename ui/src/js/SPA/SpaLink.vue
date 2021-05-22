<template>
	<a :href="target" v-on:click="navigate">
		<slot />
	</a>
</template>

<script lang="ts" >
import Vue from 'vue';
import SPA from '@/SPA/spa';

export default Vue.extend({
	props: {
		target: {
			type: String,
			default: "/"
		}
	},
	methods: {
		navigate(ev: Event) {
			ev.preventDefault();
			ev.stopPropagation();
			SPA.navigate(this.target).then((page) => page.render()).catch((error) => console.error("[SPA Link] Error: ", error));
		}
	}
})

</script>