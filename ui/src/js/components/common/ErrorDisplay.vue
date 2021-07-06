<template>
    <span v-if="errors && errors.length == 1" class="color-red margin-b-10">
        <b>Error:</b> {{ errors[0] }}
    </span>
    <span v-else-if="errors && errors.length > 1">
        <b>Errors:</b>
        <ul>
            <li v-for="error in errors" :key="error">
                {{ error }}
            </li>
        </ul>
    </span>
</template>

<script lang="ts">
import { Vue, Component, Prop, Watch } from 'vue-property-decorator';


@Component({
    components: {

    }
})
export default class extends Vue {
    @Prop({default: (): string[] => []}) private errors: string[];
}
</script>