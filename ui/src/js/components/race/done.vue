<template>
	<div class="section no-hover flex flex-vertical align-left">
		<span class="xxl">
			{{ race.name }}
		</span>
	</div>
</template>

<script lang="ts">
import Race from '@/data/Race';
import { Vue, Prop, Watch, Component } from 'vue-property-decorator';

@Component({
	components: {

	}
})
export default class extends Vue {
	@Prop({default: (): any => null}) private readonly race: Race;
}

</script>