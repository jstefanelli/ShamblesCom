<template>
  <div>
	  <p>Test component bruddah!</p>
	  <div v-if="data != null">
		  Test: {{ data.test }}
	  </div>
	  <button v-on:click="onClick">Go to test2</button>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import SPA from '@/SPA/spa';

export default Vue.extend({
	props: {
		data: {
			type: Object,
			default: null
		}
	},
	methods: {
		onClick() {
			SPA.navigate("/test2").then((page) => page.render());
		}
	}
});
</script>