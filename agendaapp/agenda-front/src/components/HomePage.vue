<template>
  <div class="container">
    <h1 class="greeting">{{ saudacao }}</h1>
    <p class="user-info">
      Usuário:
      <span>{{ userName }}</span>
    </p>

    <Card class="contacts-card">
      <template #content>
        <DataTable
          :value="contatos"
          responsiveLayout="scroll"
          class="p-datatable-sm"
          :loading="loading"
        >
          <Column field="nome" header="Nome" />
          <Column field="email" header="Email" />
          <Column field="telefone" header="Telefone" :body="formatarTelefone" />
          <Column field="tipoTelefone" header="Tipo" />
          <Column header="Ações">
            <template #body="slotProps">
              <Button
                icon="pi pi-times"
                class="p-button-rounded p-button-danger p-button-text"
                @click="deleteContato(slotProps.data, slotProps.index)"
                v-tooltip="'Excluir contato'"
              />
            </template>
          </Column>
        </DataTable>
      </template>
    </Card>

    <!-- O restante do template permanece igual -->
    <Card class="form-card">
      <template #title>
        <h3>Adicionar Contato</h3>
      </template>
      <template #content>
        <form @submit.prevent="addContato" class="contact-form">
          <div class="form-group">
            <label for="nome">Nome:</label>
            <InputText
              v-model="newContato.nome"
              id="nome"
              required
              placeholder="Fulano de Tal"
              class="p-inputtext-lg"
            />
          </div>
          <div class="form-group">
            <label for="email">Email:</label>
            <InputText
              v-model="newContato.email"
              id="email"
              required
              placeholder="exemplo@exemplo.com"
              class="p-inputtext-lg"
            />
          </div>
          <div class="form-group">
            <label for="tipoTelefone">Tipo de Telefone:</label>
            <Dropdown
              v-model="newContato.tipoTelefone"
              :options="tipoOptions"
              optionLabel="label"
              optionValue="value"
              placeholder="Selecione o tipo"
              id="tipoTelefone"
              required
              class="p-dropdown-lg"
              @change="onTipoChange"
            />
          </div>
          <div class="form-group">
            <label for="telefone">Telefone:</label>
            <InputText
              v-if="newContato.tipoTelefone === 'Celular'"
              v-model="newContato.telefone"
              id="telefone"
              v-mask="'(##) #####-####'"
              placeholder="(99) 99999-9999"
              required
              inputmode="numeric"
              class="p-inputtext-lg"
            />
            <InputText
              v-else-if="newContato.tipoTelefone === 'Fixo'"
              v-model="newContato.telefone"
              id="telefone"
              v-mask="'(##) ####-####'"
              placeholder="(99) 9999-9999"
              required
              inputmode="numeric"
              class="p-inputtext-lg"
            />
            <InputText
              v-else
              disabled
              placeholder="Selecione o tipo de telefone"
              class="p-inputtext-lg"
            />
          </div>

          <Button
            label="Adicionar"
            type="submit"
            class="submit-btn"
            :loading="submitting"
          />
        </form>
        <span v-if="formError" class="form-error">{{ formError }}</span>
      </template>
    </Card>
    <div class="status-alert" :class="{ 'is-active': !backendOnline }">
      <i class="pi pi-info-circle"></i>
      <span>
        Sistema operando com o serviço gratuito do Railway(https://railway.com).
      </span>
    </div>
  </div>
</template>

<script>
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Dropdown from 'primevue/dropdown'
import Card from 'primevue/card'
import axios from 'axios'
import Tooltip from 'primevue/tooltip'

export default {
  directives: {
    tooltip: Tooltip,
  },
  components: {
    Button,
    InputText,
    DataTable,
    Column,
    Dropdown,
    Card,
  },
  data() {
    return {
      saudacao: this.getGreeting(),
      userName: 'Wictor',
      contatos: [],
      newContato: {
        nome: '',
        email: '',
        telefone: '',
        tipoTelefone: null,
      },
      tipoOptions: [
        { label: 'Celular', value: 'Celular' },
        { label: 'Fixo', value: 'Fixo' },
      ],
      loading: false,
      submitting: false,
      formError: '',
    }
  },
  mounted() {
    this.fetchContatos()
  },
  methods: {
    getGreeting() {
      const hour = new Date().getHours()
      if (hour < 12) return 'Bom dia!'
      if (hour < 18) return 'Boa tarde!'
      return 'Boa noite!'
    },
    formatarTelefone(telefone) {
      const numeroLimpo = String(telefone).replace(/\D/g, '')
      if (numeroLimpo.length === 11) {
        return numeroLimpo.replace(/(\d{2})(\d{5})(\d{4})/, '($1) $2-$3')
      } else if (numeroLimpo.length === 10) {
        return numeroLimpo.replace(/(\d{2})(\d{4})(\d{4})/, '($1) $2-$3')
      }
      return telefone
    },
    async fetchContatos() {
      this.loading = true
      try {
        const response = await axios.get(
          'https://agendaapp-production-d1ee.up.railway.app/api/contato',
        )
        this.contatos = response.data
        this.contatos.forEach((contato) => {
          contato.telefone = this.formatarTelefone(
            contato.telefone,
            contato.tipoTelefone,
          )
        })
      } catch (error) {
        alert('Erro ao carregar os contatos. Tente novamente.')
      } finally {
        this.loading = false
      }
    },
    async addContato() {
      this.formError = ''

      if (
        !this.newContato.nome ||
        !this.newContato.email ||
        !this.newContato.telefone ||
        !this.newContato.tipoTelefone
      ) {
        this.formError = 'Por favor, preencha todos os campos.'
        return
      }

      this.submitting = true
      try {
        const contato = {
          nome: this.newContato.nome,
          email: this.newContato.email,
          telefone: this.newContato.telefone.replace(/\D/g, ''),
          tipoTelefone: this.newContato.tipoTelefone,
        }
        await axios.post('https://agendaapp-production-d1ee.up.railway.app/api/contato', contato)

        contato.telefone = this.formatarTelefone(
          contato.telefone,
          contato.tipoTelefone,
        )
        this.contatos.push(contato)
        this.fetchContatos()

        this.newContato = {
          nome: '',
          email: '',
          telefone: '',
          tipoTelefone: '',
        }
      } catch (error) {
        this.formError = 'Erro ao adicionar o contato. Tente novamente.'
      } finally {
        this.submitting = false
      }
    },
    async deleteContato(contato, index) {
      if (confirm(`Deseja realmente excluir o contato ${contato.nome}?`)) {
        try {
          await axios.delete(
            `https://agendaapp-production-d1ee.up.railway.app/api/contato/${contato.id}`,
          )
          this.contatos.splice(index, 1)
        } catch (error) {
          alert('Erro ao excluir o contato. Tente novamente.')
        }
      }
    },
    onTipoChange(event) {
      this.newContato.tipoTelefone = event.value
    },
  },
}
</script>

<style scoped>
.container {
  max-width: 900px;
  margin: 40px auto;
  padding: 0 20px;
}

.greeting {
  color: #2e7d32;
  font-size: 2.5rem;
  text-align: center;
  margin-bottom: 10px;
}

.user-info {
  text-align: center;
  font-size: 1.2rem;
  color: #555;
  margin-bottom: 30px;
}

.user-info span {
  font-weight: bold;
  color: #2e7d32;
}

.contacts-card {
  margin-bottom: 40px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  border-radius: 8px;
}

.form-card {
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  border-radius: 8px;
}

.form-card h3 {
  color: #2e7d32;
  text-align: center;
}

.contact-form {
  display: flex;
  flex-direction: column;
  gap: 20px;
  padding: 20px;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 8px;
  align-items: center;
}

.form-group label {
  font-weight: 500;
  color: #333;
}

.p-inputtext-lg,
.p-dropdown-lg {
  width: 100%;
  max-width: 400px;
}

.submit-btn {
  background-color: #4caf50;
  border: none;
  padding: 10px 20px;
  font-size: 1.1rem;
  align-self: center;
  transition: background-color 0.3s;
}

.submit-btn:hover {
  background-color: #388e3c;
}

:deep(.p-datatable .p-datatable-thead > tr > th) {
  background-color: #f5f5f5;
  color: #333;
  font-weight: 600;
}

:deep(.p-datatable .p-datatable-tbody > tr) {
  transition: background-color 0.2s;
}

:deep(.p-datatable .p-datatable-tbody > tr:hover) {
  background-color: #f9f9f9;
}

.form-error {
  color: #d32f2f;
  text-align: center;
  font-size: 1rem;
  margin-top: -10px;
}
.status-alert {
  margin-top: 40px;
  padding: 12px;
  border-radius: 4px;
  background: #fff3e0;
  color: #e65100;
  display: flex;
  align-items: center;
  gap: 8px;
  transition: all 0.3s ease;
  font-size: 0.9rem;
}

.status-alert.is-active {
  background: #ffebee;
  color: #c62828;
}

.status-alert i {
  font-size: 1.2rem;
}
</style>
