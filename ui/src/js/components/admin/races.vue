<template>
	<base-layout v-bind="index">
		<div class="fill card no-hover flex flex-vertical padding-25">
			<div class="font-m margin-b-25">
				Races:
			</div>
			<div class="flex-grow flex">
				
			</div>
		</div>
	</base-layout>
</template>

<script lang="ts">
import Season from '@/data/Season';
import Track from '@/data/Track';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';
import BaseLayout from './baseLayout.vue';

@Component({
	components: {
		BaseLayout

	}
})
export default class extends Vue {
	@Prop({default: (): any => {} }) private index: any;
	@Prop({default: (): Season => null }) private season: Season;
	@Prop({default: (): Track[] => [] }) private tracks: Track[];
}
</script>