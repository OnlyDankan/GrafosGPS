// adj = adjacente (anda para os vizinhos)
// Dictionary = busca rapida por nome chave da rua
// HashSet = tabela sem repetição, ou seja evita duplicatas nas conexoes

// tipo que reunie métodos (funções) para manipular o grafico 
public class Graph 
{
    private Dictionary<string, HashSet<string>> adj;

    public Graph()
    {
        // chamando as conexoes de adjacente
        adj = new Dictionary<string, HashSet<string>>();
    }

    // funcoes:
    public void AddStreet(string street)
    {
        if (!adj.ContainsKey(street))
            adj[street] = new HashSet<string>();
    }

    public void Connect(string a, string b)
    {
        if (adj.ContainsKey(a) && adj.ContainsKey(b))
        {
            adj[a].Add(b);
            adj[b].Add(a);
        }
    }

    public void BlockStreet(string street)
    {
        if (!adj.ContainsKey(street))
        {
            Console.WriteLine("Rua inexistente para bloquear.");
            return;
        }

        // Copia temporária para evitar erro de modificação
        var copia = adj.Keys.ToList();

        foreach (var s in copia)
            adj[s].Remove(street);

        adj.Remove(street);

        Console.WriteLine($"Rua {street} bloqueada com sucesso!");
    }

    public void UnblockStreet(string street)
    {
        // Apenas recria a rua vazia
        AddStreet(street);
        Console.WriteLine($"Rua {street} desbloqueada (sem conexões).");
    }

    public List<string> FindPath(string start, string end)
    {

         if (!adj.ContainsKey(start) || !adj.ContainsKey(end))
            return null;

        Queue<string> fila = new Queue<string>();
        Dictionary<string, string> origem = new Dictionary<string, string>();
        HashSet<string> visitado = new HashSet<string>();

        fila.Enqueue(start);
        visitado.Add(start);
        origem[start] = null;

        while (fila.Count > 0)
        {
            string atual = fila.Dequeue();

            if (atual == end)
                break;

            foreach (var viz in adj[atual])
            {
                if (!visitado.Contains(viz))
                {
                    visitado.Add(viz);
                    fila.Enqueue(viz);
                    origem[viz] = atual;
                }
            }
        }

        if (!origem.ContainsKey(end))
            return null;

        List<string> caminho = new List<string>();
        string node = end;

        while (node != null)
        {
            caminho.Add(node);
            node = origem[node];
        }

        caminho.Reverse();
        return caminho;
    }

    // quantas casas foram percorridas
    public int PathDistance(List<string> path)
    {
        if (path == null || path.Count == 0)
            return -1;

        return path.Count - 1; 
    }

    public void DisplayMap()
    {
        Console.WriteLine("\n--- MAPA ATUAL ---");

        foreach (var rua in adj)
        {
            Console.WriteLine($"{rua.Key} -> {string.Join(", ", rua.Value)}");
        }
        Console.WriteLine("------------------\n");
    }

    public void Stop()
    {
        Console.WriteLine("Encerrando sistema. Até a proxima!");
        Environment.Exit(0);
    }

    //NÃO MEXA AQUI, TÁ FUNCIONANDO SÓ POR INTERVENÇÃO DIVINA
    internal bool ConstainsStreet(string street)
    {
        return adj.ContainsKey(street);
    }
}

class Program
{
    static void Main()
    {
        bool Continua = true;
        while (Continua)
        {
            Console.WriteLine("\n Seja Bem-Vindo(a) ao GPS");

            Graph mapa = new Graph();

            // Ruas base
            mapa.AddStreet("A");
            mapa.AddStreet("B");
            mapa.AddStreet("C");
            mapa.AddStreet("D");
            mapa.AddStreet("E");
            mapa.AddStreet("F");
            mapa.AddStreet("G");
            mapa.AddStreet("H");
            mapa.AddStreet("I");
            mapa.AddStreet("J");
            mapa.AddStreet("K");
            mapa.AddStreet("L");
            mapa.AddStreet("M");
            mapa.AddStreet("N");


            // Conexões
            mapa.Connect("A", "B");
            mapa.Connect("A", "C");
            mapa.Connect("B", "D");
            mapa.Connect("C", "E");
            mapa.Connect("E", "D");
            mapa.Connect("E", "F");
            mapa.Connect("E", "G");
            mapa.Connect("E", "H");
            mapa.Connect("F", "I");
            mapa.Connect("F", "J");
            mapa.Connect("H", "K");
            mapa.Connect("H", "L");
            mapa.Connect("I", "M");
            mapa.Connect("J", "M");
            mapa.Connect("K", "N");
            mapa.Connect("L", "N");
            mapa.Connect("M", "N");


            /*Assim ficaria o mapa mentalmente:


         B
       /   \
      A     D
       \   /
         C
         |
         E
      /  |  \
     F   G   H
    / \     / \
   I   J   K   L
    \ /     \ /
     M-------N
*/

            // funcao pra ler, formatar pra maiusculo e remover espacos (evita erros)
            string configString()
            {
                return Console.ReadLine().ToUpper().Replace(" ", "");
            }

            // se o usuario digitar sair, ele para o programa *🦆 melhor mexer pra q isso possa ocorrer a qualquer momento e nao apenas no começo (dps faço isso)
            //Arrumei já bebe *🇮🇹 Eu fiz um loop, foi o único jeito que achei para arrumar, mas provavelmente tem melhores.
            string entrada;
            while (true)
            {
                Console.WriteLine("...(Digite 'sair' para encerrar ou vazio para continuar)...");
                entrada = configString();

                //Aqui o sistema ele da continuidade caso o usuário coloque um espaço como resposta
                if (string.IsNullOrEmpty(entrada)) break;

                if (entrada == "SAIR")
                {
                    mapa.Stop();
                    break;
                }

                Console.WriteLine("Entrada inválida! Digite somente 'sair' ou deixe vázio.");
            }

            //*it Deixar apenas para a versão no terminal
            Console.WriteLine("\nHá 14 ruas");
            Console.WriteLine("De A à N");

            Console.WriteLine("Qual rua está bloqueada? (Digite ou deixe vazio)");
            string block = configString();
            

            if (!string.IsNullOrWhiteSpace(block))
                mapa.BlockStreet(block);

            Console.WriteLine("Deseja desbloquear alguma rua? (Digite ou deixe vazio)");
            string unblock = configString();

            if (!string.IsNullOrWhiteSpace(unblock))
                mapa.UnblockStreet(unblock);

            mapa.DisplayMap();


            Console.Write("Rua de origem: ");
            string start;

            //Aqui é criado um loop para caso o usuário digite uma rua errada, o sistema perguntar de novo.
            while (true)
            {
             start = configString();

             if (mapa.ConstainsStreet(start))
                {
                    break;
                }   

            Console.Write("Rua inválida! Digite novamente: ");  
            }

            Console.Write("Rua de destino: ");
            string end;

            //Loop com a mesma intenção do de cima
            while (true)
            {
                end = configString();

                if (mapa.ConstainsStreet(end))
                {
                    break;
                }


            Console.Write("Rua inválida! Digite novamente: ");    
            }

            var caminho = mapa.FindPath(start, end);

            if (caminho == null)
            {
                Console.WriteLine($"O trajeto para a rua {end} está bloqueado");
            }    
            else
            {
                Console.WriteLine("Rota encontrada: " + string.Join(" -> ", caminho));

                int distancia = mapa.PathDistance(caminho);
                Console.WriteLine("Distância total: " + distancia + " ruas. ");
            }

            
            Console.Write("Você gostaria de fazer outra rota? (S/N)");
            string Nrota = Console.ReadLine();

            if (!Nrota.Equals("S", StringComparison.OrdinalIgnoreCase))
            {
                Continua = false;
                Console.WriteLine("Encerrando sistema. Até a próxima!");
            } 
            
        }
    }
    //Só Deus sabe o pq o VsCode criou e o pq isso funciona
     private static void While(bool v)
    {
        throw new NotImplementedException();
    }   

}