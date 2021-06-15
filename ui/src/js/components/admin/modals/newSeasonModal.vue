<template>
	<modal ref="myModal">
		<template slot="header">
			Add a new season:
		</template>

		<template>
			<div class="flex flex-vertical">
				<span v-if="errors && errors.Name" class="color-red margin-b-10"><b>Error:</b> {{ errors.Name }}</span>
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
import Season from '@/data/Season';
import SPA from '@/SPA/spa';
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';

@Component({
	components: {
		Modal

	}
})
export default class extends Vue {
	@Prop({default: (): Category[] => []}) public categories: Category[];
	private nextName: string = null;
	private nextCategoryId: number = 0;

	public errors: any = null;

	private modal(): any {
		return (this.$refs.myModal as any);
	}

	public open(): void {
		this.nextName = "";
		this.nextCategoryId = 0;
		this.errors = null;
		this.modal().openModal();
	}

	public close(): void {
		this.modal().closeModal();
	}

	public submit(): void {

		let nextSeason: Season = {
			id: 0,
			categoryId: this.nextCategoryId,
			category: null,
			name: this.nextName,
			last: null,
			next: null,
			races: null
		};

		let json = JSON.stringify(nextSeason);

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
			} else {
				res.json().then((data) => this.errors = data.errors).catch((err) => {
					this.errors = { "Name" : "Server error"};
					console.warn("[AddSeason] Failed to add season. Late error:", err);
					return res.text()
				}).then(result => {
					console.warn("[AddSeason] Full result: ", result);
				});
			}
		}).catch((err) => {
			console.error("[AddSeason] Failed to add season: ", err);
		})
	}
}
</script>