using System;
using System.Collections.Generic;
using System.Linq;

public class Graph
{
    private Dictionary<string, HashSet<string>> adj;

    public Graph()
    {
        adj = new Dictionary<string, HashSet<string>>();
    }

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
}

class Program
{
    static void Main()
    {
        Graph mapa = new Graph();

        // Ruas base
        mapa.AddStreet("A");
        mapa.AddStreet("B");
        mapa.AddStreet("C");
        mapa.AddStreet("D");
        mapa.AddStreet("E");

        // Conexões
        mapa.Connect("A", "B");
        mapa.Connect("A", "C");
        mapa.Connect("B", "D");
        mapa.Connect("C", "E");
        mapa.Connect("E", "D");

        Console.WriteLine("Deseja bloquear alguma rua? (Digite ou deixe vazio)");
        string block = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(block))
            mapa.BlockStreet(block);

        Console.WriteLine("Deseja desbloquear alguma rua? (Digite ou deixe vazio)");
        string unblock = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(unblock))
            mapa.UnblockStreet(unblock);

        mapa.DisplayMap();

        Console.WriteLine("Rua de origem:");
        string start = Console.ReadLine();

        Console.WriteLine("Rua de destino:");
        string end = Console.ReadLine();

        var caminho = mapa.FindPath(start, end);

        if (caminho == null)
        {
            Console.WriteLine("Nenhuma rota possível.");
        }    
        else
        {
            Console.WriteLine("Rota encontrada: " + string.Join(" -> ", caminho));

            int distancia = mapa.PathDistance(caminho);
            Console.WriteLine("Distância total: " + distancia + " ruas. ");
        }
    }
}