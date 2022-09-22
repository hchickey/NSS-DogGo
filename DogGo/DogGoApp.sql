SELECT o.Id, o.Name, o.Email, o.Address, o.Phone, o.NeighborhoodId,
d.Id, d.OwnerId, d.Name AS DogName
FROM Owner o
JOIN Dog d ON d.OwnerId = o.Id

Select * from Dog;