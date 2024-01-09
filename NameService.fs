namespace fmvc.Users

type NameService() =
    member _.GetName(id: int) = task { return $"Bobbo{id}" }
