program testeenquanto;
var oi,i:integer;
begin
	oi := 1;
	for i := 1 to 10 do
	begin
		oi := oi + i;
	end;
	oi := 1;
	for i := 10 downto 1 do
	begin
		oi := oi + i;
	end;
end.