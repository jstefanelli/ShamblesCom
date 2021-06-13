<template>
	<base-page>
		<div class="fill-w margin-fill-h padding-50 flex align-center">
			<div class="flex-grow" />
			<my-section class="flex flex-vertical padding-50 align-right">
				<span class="font-m margin-b-10 color-red" v-if="errors && errors.name">
					<b>Error:</b> {{ errors.name }}
				</span>
				<span class="font-m margin-b-5">Email:</span>
				<input type="text" class="width-350" v-model="form.email" />
				<span class="font-m margin-b-5 margin-t-10 ">Password:</span>
				<input type="password" class="width-350" v-model="form.password" />
				<div class="flex">
					<div class="flex-grow" />
					<spa-link class="button margin-t-15" target="/login" method="POST" :reqData="form" >Login</spa-link>
				</div>
			</my-section>
			<div class="flex-grow" />
		</div>
	</base-page>
	
</template>

<script lang="ts">
import SpaLink from '@/SPA/SpaLink.vue';
import BasePage from "@/components/layout/basePage.vue";
import Section from "@/components/layout/section.vue";
import { Vue, Component, Prop } from 'vue-property-decorator';

export class LoginData {
	public email: string;
	public password: string;
}

@Component({
	components: {
		SpaLink,
		BasePage,
		"my-section": Section
	}
})
export default class extends Vue {
	@Prop({default: null}) errors: any;

	form: LoginData = new LoginData();
}
</script>