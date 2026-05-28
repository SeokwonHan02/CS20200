module NimGame

open System

type GameAction =
    | Move of pile: int * count: int
    | GiveUp

let rng = Random()

let showPiles (piles: int list) =
    let rec loop (idx: int) (sublist: int list) =
        match sublist with
        | [] -> ()
        | h :: t ->
            let rec makeStones count current =
                if count = 0 then
                    ""
                else
                    let space =
                        if current > 0 && current % 5 = 0 then
                            "  "
                        else
                            ""
                    space + "● " + makeStones (count - 1) (current + 1)
            let stones = makeStones h 0
            printfn "Pile %d: (%2d) %s " idx h stones
            loop (idx + 1) t
    printfn ""
    printfn "Current piles:"
    loop 1 piles
    printfn ""

let isGameOver (piles: int list) =
    let rec check (filter: int -> bool) (sublist: int list) : bool =
        match sublist with
        | [] -> true
        | h :: t ->
            if filter h then check filter t
            else false
    check (fun x -> x = 0) piles

let applyMove (piles: int list) pile count =
    let rec mapWithIdx (idx: int) (sublist: int list) : int list =
        match sublist with
        | [] -> []
        | h :: t ->
            let nextValue = if idx = pile - 1 then h - count else h
            nextValue :: mapWithIdx (idx + 1) t
    mapWithIdx 0 piles

let readMove (piles: int list) : GameAction option =
    printf "Your move, ex) 3 2 or give up: "
    let input = Console.ReadLine()
    printfn ""

    if String.IsNullOrEmpty(input) then 
        None
    else
        if input = "give up" then 
            Some GiveUp
        else
            let parts = input.Split(' ')
            if parts.Length <> 2 then 
                None
            else
                let part1 = parts.[0]
                let part2 = parts.[1]

                let isNumeric (str: string) =
                    if str.Length = 0 then false
                    else
                        let mutable onlyDigits = true
                        for i in 0 .. (str.Length - 1) do
                            let c = str.[i]

                            if c < '0' || c > '9' then
                                onlyDigits <- false
                        onlyDigits

                if isNumeric part1 && isNumeric part2 then
                    let convertToInt (str: string) =
                        let mutable result = 0
                        for i in 0 .. (str.Length - 1) do
                            let digit = int str.[i] - int '0'
                            result <- result * 10 + digit
                        result

                    let pile = convertToInt part1
                    let count = convertToInt part2

                    if pile >= 1 && pile <= piles.Length && count > 0 && piles.[pile - 1] >= count then
                        Some (Move(pile, count))
                    else 
                        None
                else 
                    None

let rec randomAiMove (piles: int list) =
    let rec getLength sublist acc =
        match sublist with
        | [] -> acc
        | h :: t -> getLength t (acc + 1)

    let rec getValue idx sublist =
        match sublist with
        | [] -> -1
        | h :: t -> if idx = 0 then h else getValue (idx - 1) t

    let len = getLength piles 0
    let pile = rng.Next(1, len + 1)
    let stones = getValue (pile - 1) piles

    if stones > 0 then
        let count = rng.Next(1, stones + 1)
        pile, count
    else
        randomAiMove piles

let optimalAiMove (piles: int list) =
    let nimSum = List.fold (^^^) 0 piles

    if nimSum = 0 then
        randomAiMove piles
    else
        let rec findOptimalMove idx sublist =
            match sublist with
            | [] -> None
            | h :: t ->
                let target = h ^^^ nimSum
                if target < h then
                    Some (idx + 1, h - target)
                else
                    findOptimalMove (idx + 1) t

        match findOptimalMove 0 piles with
        | Some (pileNum, count) -> pileNum, count
        | None -> randomAiMove piles

[<EntryPoint>]
let main _ =
    Console.Clear()
    printfn "[NIM Game]"
    printfn ""

    let mutable choice = ""
    while choice <> "1" && choice <> "2" do
        printf "Choose AI (1: random, 2: optimal): "
        choice <- Console.ReadLine()
        if choice <> "1" && choice <> "2" then
            printfn "Invalid input"

    let mutable piles = List.init 5 (fun _ -> rng.Next(5, 15))
    let mutable turn = 1
    let mutable playerTurn = true
    let mutable finished = false

    while not finished do
        printfn ""
        printfn "[Turn %d]" turn
        showPiles piles

        if playerTurn then
            match readMove piles with
            | Some GiveUp ->
                printfn "You gave up. AI wins."
                finished <- true
            | Some (Move(pile, count)) ->
                piles <- applyMove piles pile count
                printfn "You removed %d from pile %d." count pile
                printfn ""
                if isGameOver piles then
                    printfn "You win!"
                    finished <- true
                else
                    playerTurn <- false
                    turn <- turn + 1
            | None ->
                printfn "Invalid input"

        else
            let pile, count =
                if choice = "1" then randomAiMove piles
                else optimalAiMove piles
            let aiName = if choice = "1" then "Random AI" else "Optimal AI"
            piles <- applyMove piles pile count
            printfn "%s removed %d from pile %d." aiName count pile

            if isGameOver piles then
                printfn "%s wins!" aiName
                finished <- true
            else
                playerTurn <- true
                turn <- turn + 1

    0