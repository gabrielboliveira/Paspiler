program testeenquanto2;
var oi,i:integer;
begin
	oi := 1;
	i := 1;
	while i <= 10 do
	begin
		oi := oi + i;
		i := i + 1;
	end;
	oi := 1;
	i := 10;
	while i >= 1 do
	begin
		oi := oi + i;
		i := i - 1;
	end;
end.