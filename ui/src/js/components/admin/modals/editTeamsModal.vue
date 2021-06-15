<template>
	<modal ref="myModal">
		<template slot="header">
			Add a new season:
		</template>

		<template>
			<div class="flex flex-vertical">
				<span v-if="errors && errors.name" class="color-red margin-b-10"><b>Error:</b> {{ errors.name }}</span>
				<span>Name:</span>
				<input type="text" class="margin-v-10" v-model="nextName"/>

				<span>Category:</span>
				<select class="margin-v-10" v-model="nextCategoryId">
					<option v-for="category in categories" :key="category.id" :value="category.id">
						{{ category.name }}
					</option>
				</select>
			</div>
		</template>

		<template slot="footer">
			<button @click="close()" class="passive">
				Cancel
			</button>
			<button @click="submit">
				Save
			</button>
		</template>
	</modal>
</template>

<script lang="ts">
import Modal from '@/components/modal/modal.vue';
import Category from '@/data/Category';
import Game from '@/data/Game';
import Team from '@/data/Team';
import Season from '@/data/Season';
import SPA from '@/SPA/spa';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';

@Component({
	components: {
		Modal

	}
})
export default class extends Vue {
	@Prop({default: (): Season => null}) public categories: Season;
	private currentTeam: Team;

	public errors: any = null;

	private modal(): any {
		return (this.$refs.myModal as any);
	}

	public open(): void {
		this.currentTeam = null;
		this.modal().openModal();
	}

	public close(): void {
		this.modal().closeModal();
	}

	public submit(): void {

		let json = "{}";

		fetch("/api/season", {
			method: "POST",
			headers: {
				'Content-Type': 'application/json',
				'Accept': 'application/json'
			},
			body: json,
			mode: 'cors'
		}).then((res) => {
			if (res.ok) {
				this.close();
				SPA.navigateAndRender("/admin");
			}
		}).catch((err) => {
			console.error("[AddSeason] Failed to add season: ", err);
		})
	}
}
</script>