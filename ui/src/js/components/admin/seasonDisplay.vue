<template>
	<div class="fill card no-hover flex flex-vertical padding-25">
		<div class="font-m margin-b-25">
			{{ season ? season.name : "unknown-season"}}
		</div>
		<div class="flex-grow flex">
			<div class="flex-grow">

			</div>
			<div class="flex flex-vertical margin-l-25 width-300">
				<button class="passive flex-grow margin-b-25 font-m">
					Register a new race
				</button>
				<button class="passive flex-grow margin-b-25 font-m">
					Edit an existing race
				</button>
				<button class="passive flex-grow font-m">
					Edit teams
				</button>
			</div>
		</div>
	</div>
</template>

<script lang="ts">
import Season from '@/data/Season';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';

@Component({
	components: {

	}
})
export default class extends Vue {
	@Prop({default: () : Season => null}) public season: Season;
}
</script>