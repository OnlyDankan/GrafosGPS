Sistema de Rotas em C# baseado em Grafos

Este projeto implementa um mecanismo de navegação utilizando a modelagem de grafos para representar um mapa urbano.  
As ruas são armazenadas em um Dictionary<string, HashSet<string>>, onde cada chave representa uma rua e o HashSet que contém suas conexões diretas.  
Essa estrutura permite buscas rápidas, evita duplicações e facilita alterações dinâmicas no grafo.

O sistema oferece funcionalidades essenciais para simular um GPS:

- Adição e remoção de conexões entre ruas  
- Bloqueio e desbloqueio de vias, modificando caminhos disponíveis em tempo real  
- Busca de rotas entre dois pontos, percorrendo o grafo conforme as conexões disponíveis  
- Exibição do mapa completo, incluindo todas as ligações ativas  

O foco do projeto é estudar estruturas de dados, otimização de buscas e manipulação de grafos em C#, criando uma base sólida para evoluções futuras, como:

- Algoritmos de menor caminho (Dijkstra, A*)  
- Validação automática de rotas  
- Interface visual para navegação  
- Importação de mapas externos  

Este repositório serve como um laboratório para experimentação e expansão de lógica de navegação, simulando o funcionamento interno de um GPS real.
