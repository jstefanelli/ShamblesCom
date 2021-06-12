<template>
	<component :is="dynamicComponent" :data="viewData" />
</template>

<script lang="ts">
import {Vue, Component, Prop } from 'vue-property-decorator';

@Component
export default class extends Vue{
	dynamicComponent: any = null;
	viewData: any = {};
}
</script>