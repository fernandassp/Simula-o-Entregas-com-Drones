# Simulacao-Entregas-com-Drones
Exercício técnico para o processo seletivo da DTI Digital, que consiste em um sistema para gerenciar entregas feitas por drones e seus voos para uma startup de logística. Seu foco é representar a alocação automática de pedidos em drones, levando em consideração alguns aspectos como capacidade e alcance do drone e a prioridade e localização dos pedidos. Este projeto é uma aplicação de console desenvolvida em C# no Visual Studio 2022.

## Funcionalidades
O sistema gera automaticamente 15 drones e 100 pedidos de forma aleatória, mas também é possível que o usuário registre novos drones e pedidos. Após o cadastro dos drones e pedidos, pode-se simular a sua alocação nos drones de acordo com os critérios exigidos (capacidade máxima, distância máxima alcançável, prioridade dos pedidos). Esta solução também atribui a cada drone um valor de velocidade média, em km/h, para realização do cálculo do tempo de entrega.

Além disso, o menu que é exibido ao usuário via console permite a visualização de relatórios que explicitam dados sobre o desempenho dos drones, relatando sua quantidade de entregas, tempo total gasto, tempo médio gasto por entrega e qual foi o drone mais eficiente. Também é possível visualizar a quantidade total de entregas feitas por todos os drones e relatórios de pedidos ordenados por:  
1- Sua prioridade (da mais alta para a mais baixa)  
2- Seu peso (do menor para maior)  
3- Sua distância em relação ao local de origem dos drones.  
 
A cidade, neste problema, é uma malha de coordenadas (XY). Nesta solução, o X é um inteiro que vai de 1 a 9 e o Y uma letra de A a Z. Todos os drones cadastrados no sistema têm o mesmo local de origem, e as coordenadas dos pedidos são geradas de forma aleatória. No menu de relatórios, o usuário pode também visualizar um mapa da cidade no console, uma representação simplificada dos pontos em que há entregas para serem feitas, pedidos já entregues e o ponto de origem dos drones.   


## Tecnologias
- Linguagem C# (.NET): utilizada para desenvolver toda a lógica do simulador.  
- Visual Studio 2022: IDE utilizada para o desenvolvimento do código.  
- Git: controle de versão do projeto  
- GitHub: hospedagem remota do repositório  
- xUnit: framework de testes, utilizado para escrever testes automatizados.  
- Plataformas de Inteligência Artificial: auxílio na revisão do código durante o desenvolvimento e no aprendizado de novas tecnologias (por exemplo, o framework xUnit) e técnicas de programação.  

## Como executar
Pré requisitos para executar o projeto:  
1- Ter o Visual Studio 2022 instalado

Modo de executar:  
1 -Clone o repositório do projeto via GitHub ou abra diretamente no Visual Studio.  
2- No Visual Studio, abra a solução .sln  
3- Pressione Ctrl + F5 para executar a aplicação sem debug.    
4- Interaja com o menu no console para cadastrar drones, pedidos, alocar entregas ou visualizar relatórios (para visualizar dados nos relatórios, certifique-se de antes alocar pedidos nos drones para que as entregas sejam feitas). 

### Testes com xUnit
Foi criado um projeto com o xUnit para a criação de testes automatizados. Para executá-los, vá até o menu superior: Teste, e clique em Executar Todos os Testes. Se estiver tudo funcionando, aparecerá "✅Aprovado" para todos os testes.

## Prompts utilizados na IA
Durante o desenvolvimento, foram utilizadas ferramentas de IA para esclarecer dúvidas técnicas e de interpretação, melhorar trechos de código e otimizar lógicas, além de conhecer frameworks para a implementação de testes.  

 ### Memórias utilizadas
 Estas memórias foram mantidas durante o desenvolvimento para manter o contexto do projeto:  
- Tipo de projeto: Simulação de entregas com drones.  
- Objetivo principal: Alocar pedidos em drones com o menor número de viagens possível, priorizando por peso, prioridade e distância.  
- Estrutura básica: Classes Pedido, Drone, Simulador  
- Tecnologia usada: Projeto em C# (console), utilizando Visual Studio 2022.

### Exemplos de Prompts utilizados
- "Minha função Remove está dando InvalidOperationException. Me ajude a identificar onde e por quê ocorre esse erro"  
- "Como posso criar um mapa da cidade usando apenas caracteres ASCII no console, com posições dos pedidos e drones?"
- "Como colorir o fundo do console, apenas para alguns caracteres?"
- "Me ajude a identificar o erro neste trecho de código"
- "Como posso alocar os pedidos nos drones e remover da lista de pedidos do simulador os que já foram entregues, garantindo que as variáveis que guardam as listas ordenadas apontam para o mesmo endereço de memória da lista original de pedidos?"
- "Como usar o framework xUnit para fazer os testes? Me dê exemplos de testes que poderiam ser feitos com ele"  
