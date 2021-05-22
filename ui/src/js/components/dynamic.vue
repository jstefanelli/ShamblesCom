<template>
	<component :is="dynamicComponent" :data="viewData" />
</template>

<script lang="ts">
import Vue from 'vue';

export default Vue.extend({
	data() { 
		return {
			dynamicComponent: null,
			viewData: null
		}
	}
});
</script>