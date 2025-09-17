# Parecer Técnico - Análise do Código Legado

## Principais Problemas Identificados

### 1. Ausência de Encapsulamento
- **Problema**: Todos os atributos da classe `Pedido` são públicos (`public String endereco`, `public double peso`, etc.)
- **Impacto**: Permite modificação direta dos dados sem validação, violando a integridade dos objetos
- **Risco**: Estados inconsistentes, dados corrompidos e comportamentos inesperados

### 2. Acoplamento entre Lógica e Apresentação
- **Problema**: Métodos como `gerarEtiqueta()` e `gerarResumoPedido()` misturam lógica de negócio com formatação de saída
- **Impacto**: Dificulta alterações no formato de apresentação e reutilização da lógica
- **Risco**: Modificações em apresentação podem quebrar regras de negócio

### 3. Duplicação de Lógica
- **Problema**: O método `calcularFrete()` é chamado múltiplas vezes nos métodos de apresentação
- **Impacto**: Recálculos desnecessários e inconsistências potenciais
- **Risco**: Performance degradada e bugs por inconsistência

### 4. Uso de Valores Mágicos
- **Problema**: Constantes como `1.5`, `10`, `1.2`, `1.1`, `5` estão hardcoded no código
- **Impacto**: Dificulta manutenção e compreensão das regras de negócio
- **Risco**: Erros ao modificar valores e falta de clareza sobre o significado dos números

### 5. Falta de Validações
- **Problema**: Nenhuma validação de entrada (peso negativo, tipo de frete inválido, etc.)
- **Impacto**: Sistema aceita dados inválidos silenciosamente
- **Risco**: Comportamentos inesperados e cálculos incorretos

### 6. Baixa Coesão
- **Problema**: Uma única classe com responsabilidades múltiplas (cálculo, formatação, validação)
- **Impacto**: Código difícil de manter e testar
- **Risco**: Mudanças em uma funcionalidade afetam outras não relacionadas

### 7. Má Nomenclatura
- **Problema**: Nomes genéricos como `tipoFrete` usando códigos crípticos ("EXP", "PAD", "ECO")
- **Impacto**: Código difícil de compreender sem documentação externa
- **Risco**: Erros de interpretação e manutenção incorreta

### 8. Ausência de Abstrações
- **Problema**: Estruturas rígidas if-else para diferentes tipos de frete
- **Impacto**: Dificulta extensão para novos tipos de frete
- **Risco**: Violação do princípio Aberto-Fechado, código frágil

## Impactos no Negócio

1. **Manutenção Custosa**: Alterações simples requerem modificações em múltiplos pontos
2. **Baixa Confiabilidade**: Ausência de validações pode gerar cálculos incorretos
3. **Dificuldade de Extensão**: Adicionar novos tipos de frete requer alteração do código core
4. **Testabilidade Comprometida**: Responsabilidades misturadas dificultam testes unitários

## Recomendações Imediatas

1. Implementar encapsulamento adequado
2. Separar responsabilidades em camadas distintas
3. Criar abstrações para tipos de frete
4. Implementar validações robustas
5. Extrair constantes para configuração
6. Melhorar nomenclatura seguindo convenções
7. Implementar tratamento de erros explícito
