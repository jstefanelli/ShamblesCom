<template>
	<modal ref="myModal">
		<template slot="header">
			Add a new season:
		</template>

		<template>
			<div class="flex flex-col">
				<span v-if="errors && errors.Name" class="text-red-500 mb-3"><b>Error:</b> {{ errors.Name }}</span>
				<span>Name:</span>
				<Myinput type="text" class="my-3" v-model="nextName"></Myinput>

				<span>Category:</span>
				<Myselect class="my-3" v-model="nextCategoryId">
					<option v-for="category in categories" :key="category.id" :value="category.id">
						{{ category.name }}
					</option>
				</Myselect>
			</div>
		</template>

		<template slot="footer">
			<Mybutton @click="close()" :main="false">
				Cancel
			</Mybutton>
			<Mybutton @click="submit" :main="true">
				Save
			</Mybutton>
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
import Myselect from '@/components/layout/controls/myselect.vue';
import Myinput from '@/components/layout/controls/myinput.vue';
import Mybutton from '@/components/layout/controls/mybutton.vue';

@Component({
	components: {
		Modal,
		Myselect,
		Myinput,
		Mybutton
	}
})
export default class extends Vue {
	@Prop({default: (): Category[] => []}) public categories: Category[];
	public nextName: string = null;
	public nextCategoryId: number = 0;

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