SELECT D.Nome, P.Nome, P.Salario
FROM Pessoa P JOIN Departamento D
ON P.DeptId = D.Id
WHERE P.Salario =
(
    SELECT MAX(Salario)
    FROM Pessoa
    WHERE DeptId = D.Id
)