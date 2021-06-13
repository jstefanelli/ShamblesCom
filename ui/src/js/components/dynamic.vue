<template>
	<component :is="dynamicComponent" v-bind="viewData" />
</template>

<script lang="ts">
import {Vue, Component, Prop } from 'vue-property-decorator';

@Component
export default class extends Vue{
	dynamicComponent: any = null;
	viewData: any = {};
}
</script>