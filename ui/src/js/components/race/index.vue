<template>
	<base-page>
		<upcoming v-if="isUpcoming()" :race="race" />
		<done v-else :race="race" />
	</base-page>
</template>

<script lang="ts">
import Race from '@/data/Race';
import { Vue, Prop, Watch, Component } from 'vue-property-decorator';
import BasePage from '../layout/basePage.vue';
import Done from './done.vue';
import Upcoming from './upcoming.vue';

@Component({
	components: {
		BasePage,
		Upcoming,
		Done

	}
})
export default class extends Vue {
	@Prop({default: (): any => null}) public readonly race: Race;

	public isUpcoming() {
		return new Date(this.race.dateTime) > new Date();
	}
}

</script>