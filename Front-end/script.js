/*botoes escolha comeco/fim*/

function toggleDropdown(id){
    const box = document.getElementById("opcoes-" + id);
    box.style.display = (box.style.display === "flex") ? "none" : "flex";
}

function selecionar(valor, id){
    document.getElementById("texto-" + id).innerText = valor;
    document.getElementById("opcoes-" + id).style.display = "none";
}

document.addEventListener("click", function(event){
    const dropdown = document.querySelectorAll(".dropdownOptions");
    dropdown.forEach(menu => {
        if (!menu.contains(event.target)  && !event.target.closest(".dropdownInput")){
            menu.style.display = "none";
        }
    });
});

/* ========= GRAFO BASEADO NO SEU C# ========= */
const conexoes = {
    A: ["B","C","I"],
    B: ["A","D","F"],
    C: ["A","E","H"],
    D: ["B","E","G","F","H"],
    E: ["C","D","G","I"],
    F: ["D","B","B","I"],
    G: ["D","E"],
    H: ["C","D","I"],
    I: ["H","F","A","E"],
};

/* ========= POSIÇÕES VISUAIS FIXAS ========= */
const pos = {
    A:{x:260, y:50},
    B:{x:120, y:150},
    C:{x:400, y:150},
    D:{x:120, y:270},
    E:{x:400, y:270},
    F:{x:40, y:200},
    G:{x:260, y:330},
    H:{x:480, y:200},
    I:{x:260, y:200},
};

/* ========= ESTADO DE BLOQUEIO ========= */
let bloqueado = {A:false,B:false,C:false,D:false,E:false,F:false,G:false,H:false,I:false};

const mapa = document.getElementById("mapa");

/* ========= DESENHO GERAL ========= */
function render(){

    mapa.innerHTML = "";

    // linhas
    Object.keys(conexoes).forEach(a=>{
        conexoes[a].forEach(b=>{
            if(a<b){ // evita duplicar
                const x1 = pos[a].x, y1 = pos[a].y;
                const x2 = pos[b].x, y2 = pos[b].y;

                const dx = x2-x1;
                const dy = y2-y1;
                const dist = Math.sqrt(dx*dx + dy*dy);
                const ang  = Math.atan2(dy,dx)*180/Math.PI;

                const ln = document.createElement("div");
                ln.className = "linha";
                ln.style.left  = x1+"px";
                ln.style.top   = y1+"px";
                ln.style.width = dist+"px";
                ln.style.transform = `rotate(${ang}deg)`;

                if(bloqueado[a] || bloqueado[b])
                    ln.classList.add("hidden");

                mapa.appendChild(ln);
            }
        });
    });

    // pontos
    Object.keys(pos).forEach(key=>{
        const p = document.createElement("div");
        p.className = "ponto";
        p.style.left = (pos[key].x - 15)+"px";
        p.style.top  = (pos[key].y - 15)+"px";
        p.textContent = key;

        if(bloqueado[key]) p.classList.add("bloqueado");

        p.onclick = ()=>{
            bloqueado[key] = !bloqueado[key];
            render();
        };

        mapa.appendChild(p);
    });
}

/* ========= BFS ========== */
function bfs(origem, destino){

    if(bloqueado[origem] || bloqueado[destino]) return null;

    let fila = [origem];
    let visitado = new Set([origem]);
    let anterior = {};

    anterior[origem] = null;

    while(fila.length){
        let atual = fila.shift();

        if(atual === destino) break;

        for(let viz of conexoes[atual]){
            if(!visitado.has(viz) && !bloqueado[viz]){
                visitado.add(viz);
                anterior[viz] = atual;
                fila.push(viz);
            }
        }
    }

    if(!(destino in anterior)) return null;

    let caminho = [];
    let n = destino;
    while(n !== null){
        caminho.push(n);
        n = anterior[n];
    }
    return caminho.reverse();
}

/* ========= BOTÃO BUSCAR ========= */
function encontrar(){
    let o = document.getElementById("origem").value.toUpperCase();
    let d = document.getElementById("destino").value.toUpperCase();
    const out = document.getElementById("resultado");

    if(!conexoes[o] || !conexoes[d]){
        out.innerHTML = "Origem ou destino inválidos.";
        return;
    }

    const caminho = bfs(o,d);
    if(!caminho){
        out.innerHTML = "Não existe caminho possível.";
    } else {
        out.innerHTML = `Caminho: <b>${caminho.join(" → ")}</b><br>
                         Distância: <b>${caminho.length-1}</b> ruas`;
    }
}

/* ========= RESET ========= */
function resetar(){
    bloqueado = {A:false,B:false,C:false,D:false,E:false};
    document.getElementById("origem").value="";
    document.getElementById("destino").value="";
    document.getElementById("resultado").innerHTML="";
    render();
}

render();
