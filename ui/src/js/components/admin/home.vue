<template>
	<base-layout v-bind="index">
		<div class="h-full w-full flex items-center">
			<div class="flex-grow" ></div>
			<div class="font-l2">Select a season to start adding data</div>
			<div class="flex-grow" ></div>
		</div>
	</base-layout>
</template>

<script lang="ts">
import Category from '@/data/Category';
import Game from '@/data/Game';
import Season from '@/data/Season';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import BaseLayout from './baseLayout.vue';

@Component({
	components: {
		BaseLayout
	}
})
export default class extends Vue {
	@Prop({default: () : any => {} }) public index: any;

}

</script>