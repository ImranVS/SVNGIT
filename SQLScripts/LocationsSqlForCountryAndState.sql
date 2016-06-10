
USE [vitalsigns]
GO
IF NOT EXISTS (select * from syscolumns where id = object_id('dbo.ValidLocations'))
BEGIN

CREATE TABLE [dbo].[ValidLocations](
	[Country] [varchar](250) NULL,
	[State] [varchar](250) NULL
) ON [PRIMARY]


INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Afghanistan', 'Daykundi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Afghanistan', 'Herat') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Afghanistan', 'Kabul') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Afghanistan', 'Kandahar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Afghanistan', 'Wardak') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Afghanistan', 'Zabul') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Albania', 'Berat District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Albania', 'Qarku i Durresit') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Albania', 'Qarku i Elbasanit') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Albania', 'Qarku i Tiranes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Albania', 'Qarku i Vlores') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Albania', 'Rrethi i Permetit') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Albania', 'Rrethi i Pogradecit') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Albania', 'Rrethi i Shkodres') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Annaba') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'El Tarf') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Illizi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Oran') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya d'' Alger') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Batna') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Bechar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Bejaia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Blida') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Bordj Bou Arreridj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Chlef') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Constantine') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Djelfa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Ghardaia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Jijel') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Laghouat') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Mascara') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Ouargla') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Relizane') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Saida') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Setif') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Souk Ahras') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Tamanrasset') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Tiaret') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Tipaza') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Tissemsilt') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Tizi Ouzou') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Algeria', 'Wilaya de Tlemcen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('American Samoa', 'Eastern District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Andorra', 'Andorra la Vella') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Andorra', 'Canillo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Andorra', 'Encamp') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Andorra', 'Escaldes-Engordany') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Andorra', 'La Massana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Andorra', 'Ordino') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Andorra', 'Sant Julià de Loria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Angola', 'Bengo Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Angola', 'Benguela') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Angola', 'Cabinda') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Angola', 'Cuanza Norte Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Angola', 'Cunene Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Angola', 'Huambo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Angola', 'Huila Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Angola', 'Kuando Kubango') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Angola', 'Kwanza Sul') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Angola', 'Luanda Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Angola', 'Lunda Norte Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Angola', 'Lunda Sul') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Angola', 'Malanje Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Angola', 'Moxico') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Angola', 'Namibe Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Angola', 'Provincia do Bie') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Angola', 'Provincia do Uige') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Antigua and Barbuda', 'Barbuda') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Antigua and Barbuda', 'Parish of Saint George') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Antigua and Barbuda', 'Parish of Saint John') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Antigua and Barbuda', 'Parish of Saint Mary') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Antigua and Barbuda', 'Parish of Saint Peter') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Buenos Aires') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Buenos Aires F.D.') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Chaco') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Chubut') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Cordoba') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Corrientes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Entre Rios') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Formosa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'La Pampa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Mendoza') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Neuquen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Provincia de Catamarca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Provincia de Jujuy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Provincia de La Rioja') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Provincia de Misiones') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Provincia de San Juan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Provincia de San Luis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Provincia de Tucuman') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Rio Negro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Salta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Santa Cruz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Santa Fe') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Santiago del Estero') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Argentina', 'Tierra del Fuego') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Armenia', 'Aragatsotni Marz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Armenia', 'Armaviri Marz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Armenia', 'Kotayk''i Marz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Armenia', 'Lorru Marz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Armenia', 'Syunik''i Marz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Armenia', 'Tavushi Marz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Armenia', 'Vayots'' Dzori Marz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Armenia', 'Yerevan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Australia', 'Australian Capital Territory') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Australia', 'New South Wales') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Australia', 'Northern Territory') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Australia', 'Queensland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Australia', 'South Australia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Australia', 'Tasmania') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Australia', 'Victoria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Australia', 'Western Australia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Austria', 'Burgenland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Austria', 'Carinthia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Austria', 'Lower Austria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Austria', 'Salzburg') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Austria', 'Styria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Austria', 'Tyrol') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Austria', 'Upper Austria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Austria', 'Vienna') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Austria', 'Vorarlberg') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Azerbaijan', 'Absheron Rayon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Azerbaijan', 'Baku City') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Azerbaijan', 'Goygol Rayon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Azerbaijan', 'Khachmaz Rayon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Azerbaijan', 'Nakhichevan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Azerbaijan', 'Quba Rayon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Azerbaijan', 'Qusar Rayon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Azerbaijan', 'Shaki City') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Azerbaijan', 'Sumqayit City') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bahamas', 'Bimini') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bahamas', 'Central Abaco District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bahamas', 'Central Eleuthera District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bahamas', 'City of Freeport District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bahamas', 'Harbour Island') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bahamas', 'New Providence District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bahamas', 'North Andros District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bahamas', 'Spanish Wells District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bahrain', 'Central Governorate') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bahrain', 'Manama') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bahrain', 'Muharraq') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bahrain', 'Northern') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bahrain', 'Southern Governorate') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bangladesh', 'Barisal Division') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bangladesh', 'Chittagong') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bangladesh', 'Dhaka') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bangladesh', 'Dhaka Division') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bangladesh', 'Khulna Division') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bangladesh', 'Rajshahi Division') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bangladesh', 'Rangpur Division') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Barbados', 'Christ Church') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Barbados', 'Saint Andrew') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Barbados', 'Saint James') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Barbados', 'Saint Lucy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Barbados', 'Saint Michael') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Barbados', 'Saint Thomas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belarus', 'Brest') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belarus', 'Gomel') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belarus', 'Grodnenskaya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belarus', 'Minsk') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belarus', 'Mogilev') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belarus', 'Vitebsk') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belgium', 'Antwerp Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belgium', 'Brussels Capital') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belgium', 'East Flanders Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belgium', 'Flemish') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belgium', 'Flemish Brabant Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belgium', 'Hainaut Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belgium', 'Liège Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belgium', 'Limburg Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belgium', 'Luxembourg Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belgium', 'Namur Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belgium', 'Walloon Brabant Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belgium', 'Walloon Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belgium', 'West Flanders Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belize', 'Belize District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belize', 'Cayo District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Belize', 'Stann Creek District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Benin', 'Atlantique Department') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Benin', 'Littoral') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bermuda', 'Hamilton city') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bermuda', 'Saint George') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bermuda', 'Sandys Parish') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bhutan', 'Chhukha Dzongkhag') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bhutan', 'Mongar Dzongkhag') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bhutan', 'Paro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bhutan', 'Thimphu Dzongkhag') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bolivia', 'Departamento de Chuquisaca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bolivia', 'Departamento de Cochabamba') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bolivia', 'Departamento de La Paz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bolivia', 'Departamento de Pando') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bolivia', 'Departamento de Potosi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bolivia', 'Departamento de Santa Cruz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bolivia', 'Departamento de Tarija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bolivia', 'El Beni') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bolivia', 'Oruro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bonaire', 'Bonaire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bosnia and Herzegovina', 'Brcko') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bosnia and Herzegovina', 'Federation of Bosnia and Herzegovina') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bosnia and Herzegovina', 'Republika Srpska') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Botswana', 'Central District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Botswana', 'North East District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Botswana', 'South East District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Acre') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Alagoas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Amapa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Amazonas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Bahia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Ceara') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Espirito Santo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Federal') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Goias') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Maranhao') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Mato Grosso') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Mato Grosso do Sul') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Minas Gerais') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Para') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Paraiba') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Parana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Pernambuco') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Piaui') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Rio de Janeiro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Rio Grande do Norte') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Rio Grande do Sul') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Rondonia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Roraima') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Santa Catarina') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Sao Paulo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Sergipe') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brazil', 'Tocantins') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brunei', 'Belait District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brunei', 'Brunei and Muara District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brunei', 'Temburong District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Brunei', 'Tutong District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Blagoevgrad') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Burgas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Gabrovo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Lovech') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Dobrich') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Khaskovo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Kurdzhali') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Kyustendil') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Montana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Pleven') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Razgrad') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Ruse') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Shumen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Silistra') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Sliven') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Smolyan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Stara Zagora') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Turgovishte') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Veliko Turnovo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Vidin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Vratsa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Oblast Yambol') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Pazardzhik') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Pernik') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Plovdiv') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Sofia-Capital') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Sofiya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Bulgaria', 'Varna') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Burkina Faso', 'Kadiogo Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Burkina Faso', 'Province du Boulgou') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Burkina Faso', 'Province du Houet') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Burundi', 'Bujumbura Mairie Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cambodia', 'Banteay Meanchey') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cambodia', 'Battambang') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cambodia', 'Kampong Chhnang') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cambodia', 'Kampong Speu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cambodia', 'Kampong Thom') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cambodia', 'Kandal') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cambodia', 'Phnom Penh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cambodia', 'Preah Sihanouk') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cameroon', 'Adamaoua') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cameroon', 'Centre') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cameroon', 'Littoral') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cameroon', 'North Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cameroon', 'North-West Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cameroon', 'South Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cameroon', 'South-West Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cameroon', 'West') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Canada', 'Alberta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Canada', 'British Columbia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Canada', 'Manitoba') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Canada', 'New Brunswick') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Canada', 'Newfoundland and Labrador') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Canada', 'Northwest Territories') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Canada', 'Nova Scotia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Canada', 'Nunavut') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Canada', 'Ontario') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Canada', 'Prince Edward Island') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Canada', 'Quebec') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Canada', 'Saskatchewan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Canada', 'Yukon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cape Verde', 'Concelho da Praia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Central African Republic', 'Commune de Bangui') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chad', 'Chari-Baguirmi Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chad', 'Hadjer-Lamis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chad', 'Logone Occidental Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chad', 'Ouaddai Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chile', 'Antofagasta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chile', 'Atacama') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chile', 'Aysen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chile', 'Coquimbo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chile', 'Los Lagos') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chile', 'Maule') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chile', 'Region de Arica y Parinacota') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chile', 'Region de la Araucania') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chile', 'Region de Los Rios') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chile', 'Region de Magallanes y de la Antartica Chilena') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chile', 'Region de Valparaiso') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chile', 'Region del Biobio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chile', 'Region del Libertador General Bernardo O''Higgins') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chile', 'Santiago Metropolitan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Chile', 'Tarapacá') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Anhui Sheng') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Beijing Shi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Chongqing Shi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Fujian') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Gansu Sheng') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Guangdong') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Guangxi Zhuangzu Zizhiqu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Guizhou Sheng') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Hainan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Hebei') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Heilongjiang Sheng') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Henan Sheng') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Hubei') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Hunan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Inner Mongolia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Jiangsu Sheng') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Jiangxi Sheng') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Jilin Sheng') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Liaoning') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Ningxia Huizu Zizhiqu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Qinghai Sheng') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Shaanxi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Shandong Sheng') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Shanghai Shi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Shanxi Sheng') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Sichuan Sheng') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Tianjin Shi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Tibet Autonomous Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Xinjiang Uygur Zizhiqu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Yunnan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('China', 'Zhejiang Sheng') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Antioquia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Atlántico') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Bogota D.C.') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Cundinamarca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento de Arauca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento de Bolivar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento de Boyaca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento de Caldas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento de Casanare') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento de Cordoba') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento de La Guajira') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento de Narino') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento de Norte de Santander') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento de Risaralda') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento de Santander') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento de Sucre') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento de Tolima') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento del Amazonas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento del Caqueta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento del Cauca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento del Cesar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento del Guainia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento del Guaviare') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento del Huila') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento del Magdalena') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento del Meta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento del Valle del Cauca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento del Vaupes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Departamento del Vichada') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Providencia y Santa Catalina, Departamento de Archipielago de San Andres') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Colombia', 'Quindio Department') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Comoros', 'Grande Comore') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Comoros', 'Ndzuwani') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Congo', 'Katanga Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Congo', 'Kinshasa City') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Congo', 'Nord Kivu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Congo', 'Province du Bas-Congo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Costa Rica', 'Provincia de Alajuela') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Costa Rica', 'Provincia de Cartago') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Costa Rica', 'Provincia de Guanacaste') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Costa Rica', 'Provincia de Heredia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Costa Rica', 'Provincia de Limon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Costa Rica', 'Provincia de Puntarenas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Costa Rica', 'Provincia de San Jose') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Bjelovarsko-Bilogorska Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Brodsko-Posavska Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Dubrovacko-Neretvanska Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Grad Zagreb') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Istarska Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Karlovacka Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Koprivnicko-Krizevacka Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Krapinsko-Zagorska Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Licko-Senjska Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Medimurska Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Osjecko-Baranjska Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Pozesko-Slavonska Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Primorsko-Goranska Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Sibensko-Kninska Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Sisacko-Moslavacka Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Splitsko-Dalmatinska Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Varazdinska Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Viroviticko-Podravska Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Vukovarsko-Srijemska Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Zadarska Zupanija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Croatia', 'Zagreb County') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cuba', 'Artemisa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cuba', 'La Habana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cuba', 'Las Tunas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cuba', 'Provincia de Camaguey') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cuba', 'Provincia de Ciego de Avila') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cuba', 'Provincia de Cienfuegos') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cuba', 'Provincia de Guantanamo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cuba', 'Provincia de Holguin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cuba', 'Provincia de Matanzas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cuba', 'Provincia de Pinar del Rio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cuba', 'Provincia de Sancti Spiritus') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cuba', 'Provincia de Santiago de Cuba') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cuba', 'Provincia Granma') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cyprus', 'Ammochostos') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cyprus', 'Keryneia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cyprus', 'Larnaka') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cyprus', 'Lefkosia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cyprus', 'Limassol') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Cyprus', 'Pafos') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Benešov District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Beroun District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Blansko District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Breclav District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Bruntál District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Central Bohemia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Ceská Lípa District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Cesky Keumlov') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Cheb District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Chomutov District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Chrudim District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Decín District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Domažlice District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Havlíckuv Brod District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Hlavni mesto Praha') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Hodonín District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Hradec Králové District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Jablonec nad Nisou District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Jeseník District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Jicín District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Jihocesky kraj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Jindrichuv Hradec District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Karlovarsky kraj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Kladno District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Klatovy District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Kraj Vysocina') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Kralovehradecky kraj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Kromeríž District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Kutná Hora District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Liberec District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Liberecky kraj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Litomerice District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Louny District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Melník District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Mesto Brno') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Mladá Boleslav District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Moravskoslezsky kraj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Most District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Náchod District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Nový Jicín District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Nymburk District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Okres Brno-Venkov') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Okres Ceske Budejovice') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Okres Frydek-Mistek') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Okres Jihlava') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Okres Karlovy Vary') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Okres Karvina') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Okres Kolin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Okres Ostrava-Mesto') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Okres Pardubice') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Okres Plzen-Mesto') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Okres Plzen-Sever') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Okres Praha-Vychod') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Okres Praha-Zapad') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Okres Usti nad Labem') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Olomouc District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Olomoucky kraj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Opava District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Pardubicky kraj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Pelhrimov District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Písek District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Plzensky kraj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Prachatice District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Prerov District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Príbram District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Prostejov District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Rakovník District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Rokycany District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Rychnov nad Knežnou District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Semily District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Sokolov District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'South Moravian') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Strakonice District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Šumperk District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Svitavy District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Tábor District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Tachov District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Teplice District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Trebíc District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Trutnov District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Uherské Hradište District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Ustecky kraj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Ústí nad Orlicí District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Vsetín District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Vyškov District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Ždár nad Sázavou District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Zlín') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Zlín District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Czech Republic', 'Znojmo District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Denmark', 'Capital Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Denmark', 'Central Jutland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Denmark', 'North Denmark') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Denmark', 'South Denmark') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Denmark', 'Zealand') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Djibouti', 'Djibouti Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominica', 'Saint Andrew') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominica', 'Saint George') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominica', 'Saint Patrick') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Nacional') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia de Barahona') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia de El Seibo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia de Hato Mayor') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia de Independencia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia de La Altagracia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia de La Romana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia de La Vega') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia de Monte Cristi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia de Pedernales') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia de Peravia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia de San Cristobal') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia de San Jose de Ocoa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia de San Juan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia de San Pedro de Macoris') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia de Santiago') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia de Santiago Rodriguez') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia de Santo Domingo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia Duarte') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia Espaillat') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Provincia Sanchez Ramirez') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Dominican Republic', 'Puerto Plata') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('East Timor', 'Dili') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia de Bolivar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia de Cotopaxi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia de El Oro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia de Esmeraldas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia de Francisco de Orellana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia de Imbabura') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia de Loja') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia de Los Rios') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia de Manabi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia de Morona-Santiago') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia de Napo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia de Pichincha') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia de Santa Elena') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia de Santo Domingo de los Tsachilas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia de Sucumbios') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia de Zamora-Chinchipe') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia del Azuay') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia del Canar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia del Carchi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia del Chimborazo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia del Guayas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia del Pastaza') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ecuador', 'Provincia del Tungurahua') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Alexandria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'As Suways') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Beheira Governorate') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Eastern Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Ismailia Governorate') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Luxor') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Muhafazat ad Daqahliyah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Muhafazat al Gharbiyah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Muhafazat al Jizah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Muhafazat al Minya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Muhafazat al Qahirah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Muhafazat al Qalyubiyah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Muhafazat Asyut') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Muhafazat Bani Suwayf') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Muhafazat Bur Sa`id') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Muhafazat Dumyat') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Muhafazat Shamal Sina''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Muhafazat Suhaj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Egypt', 'Red Sea') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('El Salvador', 'Departamento de Ahuachapan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('El Salvador', 'Departamento de La Libertad') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('El Salvador', 'Departamento de Morazan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('El Salvador', 'Departamento de San Miguel') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('El Salvador', 'Departamento de San Salvador') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('El Salvador', 'Departamento de Santa Ana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('El Salvador', 'Departamento de Sonsonate') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('El Salvador', 'Departamento de Usulutan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Eritrea', 'Maekel Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Estonia', 'Harju') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Estonia', 'Hiiumaa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Estonia', 'Ida-Virumaa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Estonia', 'Järvamaa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Estonia', 'Jõgevamaa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Estonia', 'Lääne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Estonia', 'Lääne-Virumaa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Estonia', 'Pärnumaa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Estonia', 'Põlvamaa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Estonia', 'Raplamaa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Estonia', 'Saare') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Estonia', 'Tartu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Estonia', 'Valgamaa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Estonia', 'Viljandimaa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Estonia', 'Võrumaa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ethiopia', 'Adis Abeba Astedader') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ethiopia', 'Afar Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ethiopia', 'Amhara') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ethiopia', 'Binshangul Gumuz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ethiopia', 'Dire Dawa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ethiopia', 'Gambela') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ethiopia', 'Harari Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ethiopia', 'Oromiya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ethiopia', 'Somali') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ethiopia', 'Southern Nations, Nationalities, and People''s Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ethiopia', 'Tigray') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Faroe Islands', 'Eysturoy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Faroe Islands', 'Norðoyar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Faroe Islands', 'Streymoy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Faroe Islands', 'Suðuroy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Federated States of Micronesia', 'State of Yap') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Fiji', 'Central') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Fiji', 'Western') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Finland', 'Eastern Finland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Finland', 'Lapponia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Finland', 'Oulu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Finland', 'Southern Finland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Finland', 'Western Finland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Ain') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Aisne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Allier') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Alpes-de-Haute-Provence') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Alpes-Maritimes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Alsace') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Ardèche') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Ardennes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Ariège') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Aube') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Aude') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Auvergne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Aveyron') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Bas-Rhin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Bouches-du-Rhône') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Calvados') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Cantal') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Centre') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Charente') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Charente-Maritime') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Cher') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Corrèze') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Cote d''Or') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Côtes-d''Armor') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Creuse') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Deux-Sèvres') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Dordogne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Doubs') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Drôme') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Essonne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Eure') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Eure-et-Loir') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Finistère') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Gard') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Gers') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Gironde') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Haute-Loire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Haute-Marne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Hautes-Alpes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Haute-Saône') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Haute-Savoie') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Hautes-Pyrénées') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Haute-Vienne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Haut-Rhin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Hauts-de-Seine') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Hérault') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Île-de-France') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Ille-et-Vilaine') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Indre') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Indre and Loire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Isère') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Jura') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Landes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Languedoc-Roussillon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Loire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Loire-Atlantique') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Loiret') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Loir-et-Cher') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Lot') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Lot-et-Garonne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Lower Normandy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Lozère') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Maine-et-Loire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Manche') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Marne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Mayenne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Meurthe et Moselle') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Meuse') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Midi-Pyrénées') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Morbihan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Moselle') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Nièvre') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'North') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Oise') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Orne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Paris') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Pas-de-Calais') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Poitou-Charentes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Provence-Alpes-Côte d''Azur') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Puy-de-Dôme') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Pyrénées-Atlantiques') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Pyrénées-Orientales') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Rhône') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Rhône-Alpes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Saône-et-Loire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Sarthe') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Savoy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Seine-et-Marne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Seine-Maritime') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Seine-Saint-Denis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Somme') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'South Corsica') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Tarn') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Tarn-et-Garonne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Territoire de Belfort') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Upper Corsica') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Upper Garonne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Val d''Oise') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Val-de-Marne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Var') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Vaucluse') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Vendée') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Vienne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Vosges') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Yonne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('France', 'Yvelines') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('French Polynesia', 'Iles du Vent') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('French Southern Territories', 'Archipel des Kerguelen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Gabon', 'Estuaire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Gabon', 'Province de la Ngounie') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Gabon', 'Province de l''Ogooue-Maritime') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Gabon', 'Province du Haut-Ogooue') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Gabon', 'Province du Moyen-Ogooue') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Gambia', 'City of Banjul') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Georgia', 'Abkhazia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Georgia', 'Ajaria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Georgia', 'Guria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Georgia', 'Imereti') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Georgia', 'K''alak''i T''bilisi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Georgia', 'Mtskheta-Mtianeti') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Georgia', 'Racha-Lechkhumi and Kvemo Svaneti') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Georgia', 'Samegrelo and Zemo Svaneti') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Georgia', 'Shida Kartli') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Germany', 'Baden-Württemberg Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Germany', 'Bavaria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Germany', 'Brandenburg') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Germany', 'Bremen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Germany', 'Hamburg') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Germany', 'Hesse') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Germany', 'Land Berlin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Germany', 'Lower Saxony') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Germany', 'Mecklenburg-Vorpommern') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Germany', 'North Rhine-Westphalia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Germany', 'Rheinland-Pfalz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Germany', 'Saarland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Germany', 'Saxony') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Germany', 'Saxony-Anhalt') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Germany', 'Schleswig-Holstein') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Germany', 'Thuringia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ghana', 'Ashanti Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ghana', 'Brong-Ahafo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ghana', 'Central Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ghana', 'Eastern Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ghana', 'Greater Accra Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ghana', 'Upper East Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ghana', 'Upper West Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ghana', 'Volta Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ghana', 'Western Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Achaea') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Aitoloakarnania') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Arcadia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Attica') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Chalkidikí') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Chania') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Chios') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Corinthia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Dodecanese') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Euboea') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Evros') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Florina') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Ilia Prefecture') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Imathia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Ioannina') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Kastoria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Kavala') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Kozani') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Laconia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Lasithi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Lefkada') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Magnesia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Messenia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Nomos Argolidos') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Nomos Artas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Nomos Dramas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Nomos Irakleiou') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Nomos Kardhitsas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Nomos Kefallinias') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Nomos Kerkyras') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Nomos Kilkis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Nomos Kykladon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Nomos Larisis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Nomos Lesvou') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Nomos Prevezis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Nomos Rodopis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Nomos Voiotias') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Nomos Zakynthou') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Pella') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Phthiotis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Pieria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Rethymno') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Sérres') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Thesprotia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Thessaloniki') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Trikala') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greece', 'Xanthi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greenland', 'Kujalleq') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greenland', 'Qaasuitsup') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greenland', 'Qeqqata') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Greenland', 'Sermersooq') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Grenada', 'Saint George') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Grenada', 'Saint John') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Grenada', 'Saint Patrick') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guam', 'Barrigada') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guam', 'Dededo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guam', 'Hagatna') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guam', 'Inarajan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guam', 'Santa Rita Municipality') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guam', 'Tamuning') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guam', 'Yigo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Departamento de Alta Verapaz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Departamento de Chimaltenango') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Departamento de Chiquimula') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Departamento de Escuintla') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Departamento de Guatemala') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Departamento de Huehuetenango') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Departamento de Izabal') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Departamento de Jutiapa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Departamento de Quetzaltenango') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Departamento de Retalhuleu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Departamento de Sacatepequez') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Departamento de San Marcos') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Departamento de Santa Rosa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Departamento de Solola') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Departamento de Totonicapan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Departamento de Zacapa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Departamento del Peten') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guatemala', 'Suchitepeque') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guernsey', 'Saint Peter Port') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guinea', 'Boke Prefecture') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guinea', 'Dabola') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guinea', 'Gaoual Prefecture') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guinea', 'Kankan Prefecture') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guinea', 'Labe Prefecture') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guinea', 'Lola') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guinea', 'Mamou Prefecture') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guinea', 'Nzerekore Prefecture') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guinea', 'Prefecture de Forecariah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guyana', 'Demerara-Mahaica Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guyana', 'East Berbice-Corentyne Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Guyana', 'Upper Demerara-Berbice Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Haiti', 'Centre') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Haiti', 'Departement de l''Ouest') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Haiti', 'Departement du Nord-Est') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Haiti', 'Nord') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Haiti', 'Sud-Est') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hashemite Kingdom of Jordan', 'Amman') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hashemite Kingdom of Jordan', 'Balqa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hashemite Kingdom of Jordan', 'Irbid') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hashemite Kingdom of Jordan', 'Madaba') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hashemite Kingdom of Jordan', 'Mafraq') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Honduras', 'Bay Islands') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Honduras', 'Departamento de Atlantida') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Honduras', 'Departamento de Comayagua') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Honduras', 'Departamento de Copan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Honduras', 'Departamento de Cortes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Honduras', 'Departamento de El Paraiso') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Honduras', 'Departamento de Francisco Morazan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Honduras', 'Departamento de Gracias a Dios') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Honduras', 'Departamento de Intibuca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Honduras', 'Departamento de Lempira') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Honduras', 'Departamento de Santa Barbara') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Honduras', 'Departamento de Valle') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Honduras', 'Departamento de Yoro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hong Kong', 'Kowloon City') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hong Kong', 'Kwon Tong') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hong Kong', 'Sha Tin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hong Kong', 'Sham Shui Po') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hong Kong', 'Southern') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hong Kong', 'Wanchai') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hong Kong', 'Wong Tai Sin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hong Kong', 'Yuen Long District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Bács-Kiskun') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Baranya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Bekes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Borsod-Abaúj-Zemplén') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Budapest fovaros') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Csongrad megye') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Fejér') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Gyor-Moson-Sopron') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Hajdú-Bihar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Heves megye') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Jász-Nagykun-Szolnok') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Komárom-Esztergom') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Nograd megye') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Pest megye') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Somogy megye') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Szabolcs-Szatmár-Bereg') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Tolna megye') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Vas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Veszprem megye') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Hungary', 'Zala') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iceland', 'Capital Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iceland', 'Northeast') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iceland', 'Northwest') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iceland', 'South') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iceland', 'Southern Peninsula') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iceland', 'West') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iceland', 'Westfjords') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Andhra Pradesh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Arunachal Pradesh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Bihar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Chandigarh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Dadra and Nagar Haveli') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Daman and Diu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Goa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Gujarat') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Haryana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Karnataka') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Kashmir') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Kerala') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Laccadives') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Madhya Pradesh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Maharashtra') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Meghalaya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Nagaland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'National Capital Territory of Delhi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Odisha') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Rajasthan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'State of Assam') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'State of Chhattisgarh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'State of Himachal Pradesh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'State of Jharkhand') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'State of Manipur') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'State of Mizoram') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'State of Punjab') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'State of Sikkim') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'State of Tripura') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Tamil Nadu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Telangana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Union Territory of Andaman and Nicobar Islands') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Union Territory of Puducherry') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Uttar Pradesh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'Uttarakhand') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('India', 'West Bengal') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Indonesia', 'North Kalimantan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Alborz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Bushehr') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'East Azarbaijan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Fars') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Hormozgan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Markazi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Mazandaran') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Ardabil') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Azarbayjan-e Gharbi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Chahar Mahal va Bakhtiari') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Esfahan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Gilan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Golestan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Hamadan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Ilam') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Kerman') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Kermanshah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Khorasan-e Jonubi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Khorasan-e Shomali') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Khuzestan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Kohgiluyeh va Bowyer Ahmad') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Kordestan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Lorestan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Qazvin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Sistan va Baluchestan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Ostan-e Tehran') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Qom') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Razavi Khorasan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Semnan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Yazd') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iran', 'Zanjan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iraq', 'An Najaf') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iraq', 'Anbar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iraq', 'Dhi Qar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iraq', 'Diyalá') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iraq', 'Mayorality of Baghdad') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iraq', 'Maysan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iraq', 'Muhafazat al Basrah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iraq', 'Muhafazat Arbil') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iraq', 'Muhafazat as Sulaymaniyah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iraq', 'Muhafazat Babil') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iraq', 'Muhafazat Kirkuk') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iraq', 'Muhafazat Ninawa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Iraq', 'Muhafazat Salah ad Din') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'Cavan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'Co Clare') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'Co Kerry') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'Co Kildare') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'Co Longford') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'Co Louth') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'Co Meath') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'County Carlow') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'County Cork') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'County Donegal') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'County Galway') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'County Kilkenny') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'County Leitrim') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'County Limerick') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'County Mayo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'County Monaghan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'County Offaly') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'County Roscommon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'County Sligo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'County Waterford') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'County Westmeath') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'County Wexford') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'County Wicklow') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'Dublin City') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'Dun Laoghaire-Rathdown') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'Fingal County') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'Laois') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'Leinster') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'Munster') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'North Tipperary') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'South Dublin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ireland', 'South Tipperary') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Israel', 'Central District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Israel', 'Haifa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Israel', 'Jerusalem') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Israel', 'Northern District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Israel', 'Southern District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Israel', 'Tel Aviv') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Ascoli Piceno') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Calabria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Campania') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Catania') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Lombardy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Milan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Monza Brianza') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Naples') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Padua') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Pesaro and Urbino') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Piedmont') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Agrigento') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Arezzo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Belluno') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Brindisi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Caltanissetta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Carbonia-Iglesias') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Enna') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Fermo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Florence') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Forlì-Cesena') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of L''Aquila') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Mantua') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Massa-Carrara') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Medio Campidano') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Messina') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Modena') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Ogliastra') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Palermo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Parma') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Pisa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Pordenone') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Ragusa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Siena') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Sondrio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Terni') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Trapani') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Province of Vibo Valentia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Alessandria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Ancona') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Asti') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Avellino') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Bari') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Barletta - Andria - Trani') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Benevento') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Bergamo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Biella') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Bologna') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Brescia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Cagliari') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Campobasso') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Caserta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Catanzaro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Chieti') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Como') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Cosenza') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Cremona') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Crotone') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Cuneo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Ferrara') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Foggia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Frosinone') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Genova') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Gorizia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Grosseto') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Imperia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Isernia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di La Spezia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Latina') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Lecce') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Lecco') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Livorno') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Lodi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Lucca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Macerata') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Matera') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Novara') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Nuoro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Olbia-Tempio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Oristano') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Pavia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Perugia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Pescara') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Piacenza') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Pistoia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Potenza') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Prato') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Ravenna') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Reggio Calabria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Reggio Emilia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Rieti') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Rimini') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Rovigo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Salerno') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Sassari') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Savona') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Taranto') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Teramo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Treviso') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Trieste') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Udine') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Varese') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Vercelli') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Verona') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Vicenza') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Provincia di Viterbo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Rome') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'South Tyrol') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Syracuse') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Trento') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Turin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Tuscany') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Valle d''Aosta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Venice') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Italy', 'Verbania') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ivory Coast', 'Lagunes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Jamaica', 'Kingston') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Jamaica', 'Parish of Clarendon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Jamaica', 'Parish of Manchester') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Jamaica', 'Parish of Saint Andrew') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Jamaica', 'Parish of Saint Ann') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Jamaica', 'Parish of Saint Catherine') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Jamaica', 'Parish of Saint James') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Jamaica', 'Parish of Saint Mary') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Jamaica', 'Parish of Westmoreland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Jamaica', 'St. Elizabeth') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Aichi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Akita') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Aomori') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Chiba') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Ehime') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Fukui') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Fukuoka') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Fukushima') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Gifu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Gunma') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Hiroshima') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Hokkaido') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Hyogo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Ibaraki') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Ishikawa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Iwate') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Kagawa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Kagoshima') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Kanagawa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Kochi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Kumamoto') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Kyoto') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Mie') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Miyagi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Miyazaki') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Nagano') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Nagasaki') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Nara') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Niigata') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Oita') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Okayama') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Okinawa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Osaka') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Saga Prefecture') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Saitama') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Shiga Prefecture') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Shimane') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Shizuoka') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Tochigi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Tokushima') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Tokyo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Tottori') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Toyama') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Wakayama') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Yamagata') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Yamaguchi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Japan', 'Yamanashi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Jersey', 'Saint Helier') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Jersey', 'Saint Peter') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kazakhstan', 'Aktyubinskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kazakhstan', 'Almaty Oblysy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kazakhstan', 'Almaty Qalasy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kazakhstan', 'Aqmola Oblysy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kazakhstan', 'Astana Qalasy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kazakhstan', 'Atyrau Oblysy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kazakhstan', 'Batys Qazaqstan Oblysy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kazakhstan', 'Bayqongyr Qalasy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kazakhstan', 'East Kazakhstan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kazakhstan', 'Mangistauskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kazakhstan', 'Pavlodar Oblysy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kazakhstan', 'Qaraghandy Oblysy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kazakhstan', 'Qostanay Oblysy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kazakhstan', 'Qyzylorda Oblysy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kazakhstan', 'Severo-Kazakhstanskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kazakhstan', 'Yuzhno-Kazakhstanskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kazakhstan', 'Zhambyl Oblysy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Garissa District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Homa Bay District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Kakamega District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Kiambu District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Kisii District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Kisumu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Kitui District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Kwale District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Machakos District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Makueni District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Mandera District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Mombasa District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Nairobi Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Nakuru District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Nandi South District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Siaya District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Tharaka District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Trans Nzoia District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kenya', 'Uasin Gishu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kiribati', 'Banaba') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kosovo', 'Komuna e Ferizajt') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kosovo', 'Komuna e Gjilanit') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kosovo', 'Komuna e Mitrovices') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kosovo', 'Komuna e Prishtines') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kosovo', 'Prizren') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kuwait', 'Al A?madi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kuwait', 'Al Asimah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kuwait', 'Al Farwaniyah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kuwait', 'Muhafazat Hawalli') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Kyrgyzstan', 'Chuyskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Laos', 'Vientiane') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Adaži') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Aizkraukles Rajons') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Aizpute') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Aluksnes Rajons') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Baldone') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Balvu Rajons') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Bauskas Rajons') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Carnikava') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Cesu Rajons') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Daugavpils') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Dobeles Rajons') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Dundaga') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Engure') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Gulbenes Rajons') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Ikškile') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Jaunjelgava') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Jekabpils Municipality') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Jelgava') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Jelgavas Rajons') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Jurmala') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Kandava') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Kegums') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Kekava') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Kraslavas Rajons') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Kuldigas Rajons') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Lielvarde') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Liepaja') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Limbazu Rajons') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Livani') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Ludzas Rajons') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Madona Municipality') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Malpils') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Marupe') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Mazsalaca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Ogre') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Olaine') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Ozolnieku Novads') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Preili Municipality') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Rezekne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Rezeknes Rajons') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Riga') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Ropazu Novads') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Rundales Novads') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Salaspils Novads') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Saldus Municipality') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Siguldas Novads') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Smiltenes Novads') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Strencu Novads') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Talsi Municipality') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Tukuma Rajons') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Valka Municipality') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Valmiera District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Varaklanu Novads') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Ventspils Municipality') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Viesites Novads') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Latvia', 'Zilupes Novads') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Lebanon', 'Beyrouth') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Lebanon', 'Mohafazat Aakkar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Lebanon', 'Mohafazat Baalbek-Hermel') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Lebanon', 'Mohafazat Liban-Nord') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Lebanon', 'Mohafazat Liban-Sud') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Lebanon', 'Mohafazat Mont-Liban') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Lesotho', 'Maseru') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Liberia', 'Montserrado County') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Liberia', 'River Gee County') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Libya', 'Sha`biyat Banghazi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Libya', 'Sha`biyat Misratah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Libya', 'Sha`biyat Sabha') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Libya', 'Tripoli') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Liechtenstein', 'Balzers') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Liechtenstein', 'Eschen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Liechtenstein', 'Gemeinde Gamprin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Liechtenstein', 'Mauren') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Liechtenstein', 'Planken') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Liechtenstein', 'Ruggell') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Liechtenstein', 'Schaan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Liechtenstein', 'Schellenberg') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Liechtenstein', 'Triesen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Liechtenstein', 'Triesenberg') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Liechtenstein', 'Vaduz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Luxembourg', 'District de Diekirch') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Luxembourg', 'District de Grevenmacher') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Luxembourg', 'District de Luxembourg') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Berovo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Bitola') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Bogdanci') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Cair') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Debar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Demir Hisar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Gevgelija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Gostivar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Ilinden') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Kavadarci') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Kumanovo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Makedonska Kamenica') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Negotino') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Novo Selo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Ohrid') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Opstina Delcevo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Opstina Karpos') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Opstina Kicevo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Opstina Kocani') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Opstina Probistip') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Opstina Radovis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Opstina Stip') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Prilep') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Struga') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Strumica') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Tetovo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Macedonia', 'Veles') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Madagascar', 'Analamanga Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Madagascar', 'Atsimo-Andrefana Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Madagascar', 'Atsinanana Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Madagascar', 'Diana Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Madagascar', 'Upper Matsiatra') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Madagascar', 'Vakinankaratra Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malawi', 'Blantyre District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malawi', 'Lilongwe District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malawi', 'Mzimba District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malaysia', 'Johor') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malaysia', 'Kedah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malaysia', 'Kelantan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malaysia', 'Kuala Lumpur') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malaysia', 'Labuan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malaysia', 'Melaka') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malaysia', 'Negeri Sembilan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malaysia', 'Pahang') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malaysia', 'Perak') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malaysia', 'Perlis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malaysia', 'Pulau Pinang') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malaysia', 'Putrajaya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malaysia', 'Sabah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malaysia', 'Sarawak') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malaysia', 'Selangor') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malaysia', 'Terengganu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Maldives', 'Maale') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mali', 'Bamako Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Attard') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Balzan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Birkirkara') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Birzebbuga') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Bormla') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Dingli') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Ghajnsielem') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Hal Gharghur') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Hal Ghaxaq') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Haz-Zabbar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Haz-Zebbug') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Il-Belt Valletta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Il-Birgu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Il-Fgura') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Il-Fontana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Il-Furjana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Il-Gudja') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Il-Gzira') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Il-Hamrun') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Il-Marsa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Il-Mellieha') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Il-Mosta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Il-Munxar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Il-Qrendi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'In-Nadur') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'In-Naxxar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Ir-Rabat') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Is-Siggiewi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Is-Swieqi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Ix-Xaghra') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Ix-Xewkija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Iz-Zebbug') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Iz-Zejtun') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Iz-Zurrieq') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Kirkop') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'L-Gharb') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'L-Ghasri') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Lija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'L-Iklin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'L-Imdina') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'L-Imgarr') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'L-Imqabba') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'L-Imsida') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'L-Imtarfa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'L-Isla') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Luqa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Marsaskala') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Marsaxlokk') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Paola') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Pembroke') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Qormi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Safi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Saint John') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Saint Julian') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Saint Lawrence') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Saint Lucia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Saint Paul’s Bay') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Saint Venera') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Sannat') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Ta'' Xbiex') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Tal-Pieta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Tarxien') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Tas-Sliema') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Malta', 'Victoria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Marshall Islands', 'Majuro Atoll') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mauritania', 'District de Nouakchott') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mauritius', 'Black River District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mauritius', 'Moka District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mauritius', 'Plaines Wilhems District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mauritius', 'Port Louis District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mauritius', 'Riviere du Rempart District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mauritius', 'Rodrigues') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mauritius', 'Savanne District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mayotte', 'Chiconi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mayotte', 'Dzaoudzi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mayotte', 'Koungou') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mayotte', 'Mamoudzou') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mayotte', 'Ouangani') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mayotte', 'Pamandzi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mayotte', 'Tsingoni') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Aguascalientes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Baja California Sur') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Campeche') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Chiapas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Chihuahua') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Coahuila') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Colima') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Durango') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Estado de Baja California') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Estado de Mexico') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Guanajuato') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Guerrero') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Hidalgo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Jalisco') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Mexico City') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Michoacán') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Morelos') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Nayarit') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Nuevo León') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Oaxaca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Puebla') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Querétaro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Quintana Roo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'San Luis Potosí') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Sinaloa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Sonora') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Tabasco') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Tamaulipas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Tlaxcala') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Veracruz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Yucatán') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mexico', 'Zacatecas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Arhangay Aymag') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Bayanhongor Aymag') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Bayan-Olgiy Aymag') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Bulgan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Central Aimak') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Darhan-Uul Aymag') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Dzavhan Aymag') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'East Aimak') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'East Gobi Aymag') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Govi-Altay Aymag') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Govi-Sumber') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Hentiy Aymag') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Hovd') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Hovsgol Aymag') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Middle Govi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Ömnögovi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Övörhangay') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Selenge Aymag') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Suhbaatar Aymag') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mongolia', 'Ulaanbaatar Hot') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Montenegro', 'Budva') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Montenegro', 'Herceg Novi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Montenegro', 'Kotor') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Montenegro', 'Opstina Niksic') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Montenegro', 'Pljevlja') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Montenegro', 'Podgorica') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Montenegro', 'Ulcinj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Montserrat', 'Parish of Saint Peter') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Agadir-Ida-ou-Tnan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Azilal Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Beni-Mellal') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Casablanca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Chaouia-Ouardigha') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'El-Hajeb') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'El-Jadida') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Errachidia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Fes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Gharb-Chrarda-Beni Hssen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Guelmim-Es Semara') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Kenitra Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Khemisset') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Khouribga Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Laayoune-Boujdour-Sakia El Hamra') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Larache') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Marrakech-Tensift-Al Haouz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Meknes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Mohammedia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Nador') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Oriental') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Oujda-Angad') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Rabat') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Region de Rabat-Sale-Zemmour-Zaer') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Region de Souss-Massa-Draa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Region de Tanger-Tetouan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Region du Grand Casablanca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Safi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Sefrou') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Settat Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Skhirate-Temara') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Tadla-Azilal') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Tanger-Assilah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Taza-Al Hoceima-Taounate') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Morocco', 'Tiznit Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mozambique', 'Cabo Delgado Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mozambique', 'Cidade de Maputo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mozambique', 'Maputo Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mozambique', 'Nampula') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mozambique', 'Provincia de Zambezia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mozambique', 'Sofala Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Mozambique', 'Tete') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Myanmar [Burma]', 'Kayin State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Myanmar [Burma]', 'Magway Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Myanmar [Burma]', 'Mandalay Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Myanmar [Burma]', 'Shan State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Myanmar [Burma]', 'Yangon Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Namibia', 'Erongo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Namibia', 'Karas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Namibia', 'Khomas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Namibia', 'Kunene') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Namibia', 'Omusati') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Namibia', 'Oshana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Namibia', 'Oshikoto') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Namibia', 'Otjozondjupa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Namibia', 'Zambezi Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nauru', 'Anabar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nepal', 'Bagmati Zone') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nepal', 'Lumbini Zone') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Netherlands', 'Friesland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Netherlands', 'Groningen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Netherlands', 'Limburg') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Netherlands', 'North Brabant') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Netherlands', 'North Holland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Netherlands', 'Provincie Drenthe') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Netherlands', 'Provincie Flevoland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Netherlands', 'Provincie Gelderland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Netherlands', 'Provincie Overijssel') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Netherlands', 'Provincie Utrecht') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Netherlands', 'Provincie Zeeland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Netherlands', 'South Holland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Caledonia', 'South Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Zealand', 'Auckland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Zealand', 'Bay of Plenty') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Zealand', 'Canterbury') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Zealand', 'Chatham Islands') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Zealand', 'Gisborne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Zealand', 'Hawke''s Bay') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Zealand', 'Manawatu-Wanganui') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Zealand', 'Marlborough') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Zealand', 'Nelson') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Zealand', 'Northland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Zealand', 'Otago') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Zealand', 'Southland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Zealand', 'Taranaki') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Zealand', 'Tasman') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Zealand', 'Waikato') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Zealand', 'Wellington') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('New Zealand', 'West Coast') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nicaragua', 'Departamento de Chinandega') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nicaragua', 'Departamento de Esteli') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nicaragua', 'Departamento de Granada') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nicaragua', 'Departamento de Jinotega') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nicaragua', 'Departamento de Leon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nicaragua', 'Departamento de Managua') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nicaragua', 'Departamento de Masaya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nicaragua', 'Departamento de Matagalpa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nicaragua', 'Departamento de Nueva Segovia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nicaragua', 'Departamento de Rivas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nicaragua', 'Region Autonoma Atlantico Sur') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Niger', 'Niamey') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Abia State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Adamawa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Akwa Ibom State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Bayelsa State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Benue State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Cross River State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Delta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Ebonyi State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Edo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Ekiti State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Enugu State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Federal Capital Territory') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Imo State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Kaduna State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Kano State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Katsina State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Kebbi State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Kogi State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Kwara State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Lagos') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Niger State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Ogun State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Ondo State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Osun State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Oyo State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Plateau State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Rivers State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Sokoto State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Taraba State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Yobe State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Nigeria', 'Zamfara State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('North Korea', 'Pyongyang') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Northern Mariana Islands', 'Saipan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Akershus') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Aust-Agder') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Buskerud') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Finnmark Fylke') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Hedmark') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Hordaland Fylke') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'More og Romsdal fylke') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Nordland Fylke') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Nord-Trondelag Fylke') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Oppland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Oslo County') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Østfold') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Rogaland Fylke') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Sogn og Fjordane Fylke') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Sor-Trondelag Fylke') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Telemark') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Troms Fylke') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Vest-Agder Fylke') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Norway', 'Vestfold') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Oman', 'Al Batinah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Oman', 'Ash Sharqiyah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Oman', 'Muhafazat ad Dakhiliyah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Oman', 'Muhafazat Masqat') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Oman', 'Muhafazat Zufar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Pakistan', 'Azad Kashmir') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Pakistan', 'Balochistan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Pakistan', 'Federally Administered Tribal Areas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Pakistan', 'Gilgit-Baltistan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Pakistan', 'Islamabad Capital Territory') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Pakistan', 'Khyber Pakhtunkhwa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Pakistan', 'Punjab') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Pakistan', 'Sindh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Palestine', 'Gaza Strip') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Palestine', 'West Bank') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Panama', 'Embera-Wounaan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Panama', 'Guna Yala') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Panama', 'Ngoebe-Bugle') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Panama', 'Provincia de Bocas del Toro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Panama', 'Provincia de Chiriqui') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Panama', 'Provincia de Cocle') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Panama', 'Provincia de Colon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Panama', 'Provincia de Herrera') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Panama', 'Provincia de Los Santos') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Panama', 'Provincia de Panama') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Panama', 'Provincia de Veraguas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Panama', 'Provincia del Darien') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'Bougainville') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'Central Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'Chimbu Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'East New Britain Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'East Sepik Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'Eastern Highlands Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'Enga Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'Gulf Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'Madang Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'Manus Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'Milne Bay Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'Morobe Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'National Capital') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'New Ireland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'Northern Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'Southern Highlands Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'West New Britain Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'West Sepik Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'Western Highlands Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Papua New Guinea', 'Western Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Paraguay', 'Asuncion') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Paraguay', 'Departamento Central') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Paraguay', 'Departamento de Alto Paraguay') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Paraguay', 'Departamento de Boqueron') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Paraguay', 'Departamento de Caaguazu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Paraguay', 'Departamento de Caazapa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Paraguay', 'Departamento de Canindeyu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Paraguay', 'Departamento de Concepcion') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Paraguay', 'Departamento de Itapua') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Paraguay', 'Departamento de Misiones') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Paraguay', 'Departamento de Presidente Hayes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Paraguay', 'Departamento de San Pedro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Paraguay', 'Departamento del Alto Parana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Paraguay', 'Departamento del Guaira') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Amazonas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Ancash') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Apurimac') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Arequipa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Ayacucho') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Cajamarca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Callao') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Cusco') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Departamento de Moquegua') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Huancavelica') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Ica') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Junin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'La Libertad') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Lambayeque') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Lima') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Loreto') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Madre de Dios') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Pasco') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Piura') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Provincia de Lima') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Puno') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Region de Huanuco') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Region de San Martin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Tacna') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Tumbes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Peru', 'Ucayali') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Autonomous Region in Muslim Mindanao') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Bicol') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Calabarzon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Caraga') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Central Luzon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Central Visayas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Cordillera') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Davao') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Eastern Samar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Eastern Visayas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Ilocos') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Mimaropa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'National Capital Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Northern Samar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Oriental Mindoro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of  Zamboanga del Sur') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Abra') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Agusan del Sur') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Aklan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Albay') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Basilan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Bataan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Batangas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Benguet') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Bohol') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Bukidnon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Bulacan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Cagayan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Camarines Sur') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Capiz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Cavite') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Cebu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Davao del Norte') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Davao del Sur') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Davao Oriental') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Ilocos Norte') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Ilocos Sur') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Iloilo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Isabela') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of La Union') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Laguna') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Lanao del Norte') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Leyte') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Maguindanao') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Marinduque') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Masbate') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Mindoro Occidental') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Misamis Occidental') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Misamis Oriental') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Negros Occidental') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Negros Oriental') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of North Cotabato') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Nueva Ecija') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Nueva Vizcaya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Palawan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Pampanga') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Pangasinan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Quezon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Rizal') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Romblon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Samar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Siquijor') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Sorsogon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of South Cotabato') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Sultan Kudarat') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Sulu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Surigao del Norte') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Tarlac') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Zambales') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Province of Zamboanga del Norte') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Soccsksargen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Southern Leyte') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Western Visayas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Philippines', 'Zamboanga Peninsula') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Poland', 'Greater Poland Voivodeship') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Poland', 'Kujawsko-Pomorskie') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Poland', 'Lesser Poland Voivodeship') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Poland', 'Lódz Voivodeship') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Poland', 'Lower Silesian Voivodeship') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Poland', 'Lublin Voivodeship') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Poland', 'Lubusz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Poland', 'Masovian Voivodeship') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Poland', 'Opole Voivodeship') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Poland', 'Podlasie') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Poland', 'Pomeranian Voivodeship') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Poland', 'Silesian Voivodeship') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Poland', 'Subcarpathian Voivodeship') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Poland', 'Swietokrzyskie') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Poland', 'Warmian-Masurian Voivodeship') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Poland', 'West Pomeranian Voivodeship') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Aveiro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Azores') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Bragança') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Distrito da Guarda') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Distrito de Beja') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Distrito de Braga') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Distrito de Castelo Branco') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Distrito de Coimbra') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Distrito de Evora') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Distrito de Faro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Distrito de Leiria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Distrito de Portalegre') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Distrito de Santarem') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Distrito de Setubal') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Distrito de Viana do Castelo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Distrito de Vila Real') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Distrito de Viseu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Distrito do Porto') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Lisbon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Portugal', 'Madeira') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Adjuntas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Aguada') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Aguadilla') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Aguas Buenas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Aibonito') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Anasco') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Arecibo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Arroyo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Barceloneta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Barranquitas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Bayamon Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Cabo Rojo Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Caguas Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Camuy Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Canovanas Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Carolina Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Catano Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Cayey Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Ceiba Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Ciales Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Cidra Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Coamo Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Corozal Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Culebra Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Dorado Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Fajardo Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Guanica Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Guayama Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Guayanilla Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Guaynabo Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Gurabo Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Hatillo Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Hormigueros Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Humacao Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Juana Diaz Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Lajas Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Lares Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Las Piedras Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Loiza Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Luquillo Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Manati Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Maricao Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Maunabo Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Mayagueez Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Moca Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Morovis Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Municipio de Isabela') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Municipio de Jayuya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Municipio de Juncos') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Naguabo Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Naranjito Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Orocovis Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Patillas Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Penuelas Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Ponce Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Quebradillas Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Rincon Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Rio Grande Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Sabana Grande Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Salinas Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'San German Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'San Juan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'San Lorenzo Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'San Sebastian Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Santa Isabel Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Toa Alta Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Toa Baja Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Trujillo Alto Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Utuado Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Vega Alta Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Vega Baja Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Vieques Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Villalba Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Yabucoa Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Puerto Rico', 'Yauco Municipio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Qatar', 'Al Wakrah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Qatar', 'Baladiyat ad Dawhah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Qatar', 'Baladiyat ar Rayyan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Korea', 'Busan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Korea', 'Chungcheongbuk-do') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Korea', 'Chungcheongnam-do') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Korea', 'Daegu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Korea', 'Daejeon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Korea', 'Gangwon-do') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Korea', 'Gwangju') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Korea', 'Gyeonggi-do') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Korea', 'Gyeongsangbuk-do') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Korea', 'Gyeongsangnam-do') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Korea', 'Incheon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Korea', 'Jeju-do') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Korea', 'Jeollabuk-do') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Korea', 'Jeollanam-do') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Korea', 'Seoul') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Korea', 'Ulsan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Lithuania', 'Alytaus Apskritis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Lithuania', 'Kauno Apskritis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Lithuania', 'Klaipedos Apskritis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Lithuania', 'Marijampoles Apskritis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Lithuania', 'Panevežys') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Lithuania', 'Siauliu Apskritis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Lithuania', 'Taurages Apskritis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Lithuania', 'Telsiu Apskritis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Lithuania', 'Utenos Apskritis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Lithuania', 'Vilnius County') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Anenii Noi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Cahul') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Criuleni') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Donduseni') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Drochia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Gagauzia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Hîncesti') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Laloveni') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Municipiul Balti') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Municipiul Bender') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Municipiul Chisinau') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Nisporeni') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Orhei') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Raionul Calarasi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Raionul Edinet') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Raionul Soroca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Stra?eni') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of Moldova', 'Unitatea Teritoriala din Stinga Nistrului') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of the Congo', 'Commune de Brazzaville') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Republic of the Congo', 'Sangha') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Arad') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Bihor') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Bucuresti') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Constanta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Covasna') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Dolj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Giurgiu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Gorj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Harghita') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Hunedoara') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Ilfov') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Alba') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Arges') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Bacau') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Bistrita-Nasaud') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Botosani') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Braila') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Brasov') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Buzau') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Calarasi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Caras-Severin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Cluj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Dambovita') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Galati') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Ialomita') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Iasi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Mehedinti') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Mures') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Neamt') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Salaj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Sibiu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Timis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Judetul Valcea') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Maramures') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Olt') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Prahova') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Satu Mare') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Suceava') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Teleorman') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Tulcea') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Vaslui') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Romania', 'Vrancea') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Altai Krai') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Amurskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Arkhangelskaya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Astrakhanskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Bashkortostan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Belgorodskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Bryanskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Chechnya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Chelyabinsk') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Chukotskiy Avtonomnyy Okrug') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Chuvashia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Dagestan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Irkutskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Ivanovskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Jewish Autonomous Oblast') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Kabardino-Balkarskaya Respublika') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Kaliningradskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Kalmykiya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Kaluzhskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Kamtchatski Kray') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Karachayevo-Cherkesiya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Kemerovskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Khabarovsk Krai') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Khanty-Mansiyskiy Avtonomnyy Okrug-Yugra') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Kirovskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Komi Republic') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Kostromskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Krasnodarskiy Kray') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Krasnoyarskiy Kray') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Kurganskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Kurskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Leningrad') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Lipetskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Magadanskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Moscow') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Moskovskaya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Murmansk') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Nenetskiy Avtonomnyy Okrug') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Nizhegorodskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'North Ossetia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Novgorodskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Novosibirskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Omskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Orenburgskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Orlovskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Penzenskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Perm Krai') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Primorskiy Kray') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Pskovskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Respublika Adygeya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Respublika Altay') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Respublika Buryatiya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Respublika Ingushetiya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Respublika Kareliya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Respublika Khakasiya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Respublika Mariy-El') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Respublika Mordoviya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Respublika Sakha (Yakutiya)') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Respublika Tyva') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Rostov') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Ryazanskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Sakhalinskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Samarskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Saratovskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Smolenskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'St.-Petersburg') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Stavropol''skiy Kray') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Sverdlovskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Tambovskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Tatarstan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Tomskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Tul''skaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Tverskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Tyumenskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Udmurtskaya Respublika') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Ulyanovsk Oblast') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Vladimirskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Volgogradskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Vologodskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Voronezhskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Yamalo-Nenetskiy Avtonomnyy Okrug') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Yaroslavskaya Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Russia', 'Zabaykal''skiy Kray') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Rwanda', 'Kigali') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saint Helena', 'Saint Helena') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saint Helena', 'Tristan da Cunha') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saint Lucia', 'Anse-la-Raye') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saint Lucia', 'Castries Quarter') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saint Lucia', 'Choiseul Quarter') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saint Lucia', 'Gros-Islet') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saint Lucia', 'Vieux-Fort') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saint Vincent and the Grenadines', 'Grenadines') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saint Vincent and the Grenadines', 'Parish of Charlotte') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saint Vincent and the Grenadines', 'Parish of Saint George') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Samoa', 'Fa`asaleleaga') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Samoa', 'Tuamasaga') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('San Marino', 'Castello di Acquaviva') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('San Marino', 'Castello di Fiorentino') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('San Marino', 'Castello di San Marino Citta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('San Marino', 'Serravalle') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('São Tomé and Príncipe', 'São Tomé Island') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saudi Arabia', 'Al Jawf') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saudi Arabia', 'Al Madinah al Munawwarah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saudi Arabia', 'Al-Qassim') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saudi Arabia', 'Ar Riya?') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saudi Arabia', 'Eastern Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saudi Arabia', 'Jizan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saudi Arabia', 'Makkah Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saudi Arabia', 'Mintaqat `Asir') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saudi Arabia', 'Mintaqat al Bahah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saudi Arabia', 'Mintaqat Ha''il') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saudi Arabia', 'Mintaqat Najran') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saudi Arabia', 'Mintaqat Tabuk') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Saudi Arabia', 'Northern Borders') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Senegal', 'Dakar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Senegal', 'Fatick') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Senegal', 'Kaolack') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Senegal', 'Kolda') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Senegal', 'Louga') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Senegal', 'Region de Kaffrine') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Senegal', 'Region de Kedougou') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Senegal', 'Region de Sedhiou') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Senegal', 'Saint-Louis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Senegal', 'Tambacounda') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Serbia', 'Autonomna Pokrajina Vojvodina') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Serbia', 'Central Serbia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Seychelles', 'English River') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Seychelles', 'Takamaka') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sierra Leone', 'Western Area') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Slovakia', 'Banskobystricky kraj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Slovakia', 'Bratislavsky kraj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Slovakia', 'Kosicky kraj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Slovakia', 'Nitriansky kraj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Slovakia', 'Presovsky kraj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Slovakia', 'Trenciansky kraj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Slovakia', 'Trnavsky kraj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Slovakia', 'Zilinsky kraj') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Slovenia', 'Log–Dragomer') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Slovenia', 'Obcina Poljcane') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Slovenia', 'Obcina Sredisce ob Dravi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Slovenia', 'Obcina Starse') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Slovenia', 'Obcina Straza') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Solomon Islands', 'Guadalcanal Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Somalia', 'Banaadir') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Somalia', 'Woqooyi Galbeed') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('South Africa', 'Eastern Cape') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('South Africa', 'Gauteng') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('South Africa', 'KwaZulu-Natal') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('South Africa', 'Limpopo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('South Africa', 'Mpumalanga') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('South Africa', 'Northern Cape') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('South Africa', 'Orange Free State') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('South Africa', 'Province of North West') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('South Africa', 'Province of the Western Cape') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('South Sudan', 'Central Equatoria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'A Coruña') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Albacete') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Alicante') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Almeria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Andalusia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Araba / Álava') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Asturias') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Avila') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Badajoz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Balearic Islands') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Barcelona') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Basque Country') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Biscay') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Burgos') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Caceres') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Cadiz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Canary Islands') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Cantabria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Castellon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Catalonia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Ceuta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Ciudad Real') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Cordoba') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Cuenca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Galicia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Gipuzkoa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Girona') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Granada') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Guadalajara') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Huelva') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Huesca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Jaen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'La Rioja') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Las Palmas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Leon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Lleida') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Lugo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Madrid') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Malaga') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Melilla') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Murcia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Navarre') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Ourense') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Palencia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Pontevedra') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Salamanca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Santa Cruz de Tenerife') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Saragossa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Segovia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Seville') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Soria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Tarragona') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Teruel') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Toledo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Valencia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Valladolid') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Spain', 'Zamora') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sri Lanka', 'Central Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sri Lanka', 'Eastern Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sri Lanka', 'North Central Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sri Lanka', 'North Western Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sri Lanka', 'Province of Uva') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sri Lanka', 'Western Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sudan', 'Kassala') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sudan', 'Khartoum') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sudan', 'Northern Darfur') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sudan', 'River Nile') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sudan', 'Southern Darfur') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sudan', 'Southern Kordofan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Suriname', 'Distrikt Brokopondo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Suriname', 'Distrikt Commewijne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Suriname', 'Distrikt Coronie') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Suriname', 'Distrikt Marowijne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Suriname', 'Distrikt Para') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Suriname', 'Distrikt Paramaribo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Suriname', 'Distrikt Saramacca') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Suriname', 'Distrikt Sipaliwini') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Svalbard and Jan Mayen', 'Svalbard') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Swaziland', 'Hhohho District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Swaziland', 'Manzini District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Blekinge') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Dalarna') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Gävleborg') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Gotland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Halland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Jämtland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Jönköping') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Kalmar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Kronoberg') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Norrbotten') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Örebro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Östergötland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Skåne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Södermanland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Stockholm') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Uppsala') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Värmland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Västerbotten') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Västernorrland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Västmanland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Sweden', 'Västra Götaland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Aargau') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Appenzell Ausserrhoden') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Appenzell Innerrhoden') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Basel-City') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Basel-Landschaft') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Bern') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Fribourg') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Geneva') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Glarus') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Grisons') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Jura') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Lucerne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Neuchâtel') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Nidwalden') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Obwalden') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Saint Gallen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Schaffhausen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Schwyz') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Solothurn') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Thurgau') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Ticino') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Uri') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Valais') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Vaud') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Zug') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Switzerland', 'Zurich') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Syria', 'Aleppo Governorate') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Syria', 'As-Suwayda Governorate') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Syria', 'Damascus Governorate') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Syria', 'Hama Governorate') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Syria', 'Latakia Governorate') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Changhua') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Chiayi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Fukien') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Hsinchu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Hualien') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Kaohsiung') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Keelung') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Miaoli') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Nantou') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Penghu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Pingtung') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Taichung') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Tainan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Taipei') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'T''ai-pei Shih') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Taitung') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Taiwan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Taoyuan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Yilan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Taiwan', 'Yunlin County') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tajikistan', 'Gorno-Badakhshan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tanzania', 'Arusha') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tanzania', 'Dar es Salaam Region') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tanzania', 'Dodoma') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tanzania', 'Kagera') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tanzania', 'Morogoro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tanzania', 'Mwanza') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tanzania', 'Njombe') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tanzania', 'Pemba North') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tanzania', 'Tanga') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tanzania', 'Zanzibar Urban/West') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Bangkok') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Amnat Charoen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Ang Thong') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Buriram') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Chachoengsao') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Chai Nat') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Chaiyaphum') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Chanthaburi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Chiang Rai') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Chon Buri') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Chumphon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Kalasin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Kamphaeng Phet') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Kanchanaburi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Khon Kaen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Krabi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Lampang') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Lamphun') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Loei') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Lop Buri') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Mae Hong Son') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Maha Sarakham') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Mukdahan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Nakhon Nayok') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Nakhon Pathom') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Nakhon Ratchasima') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Nakhon Sawan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Nakhon Si Thammarat') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Nan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Narathiwat') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Nong Bua Lamphu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Nong Khai') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Nonthaburi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Pathum Thani') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Pattani') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Phangnga') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Phatthalung') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Phayao') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Phetchabun') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Phetchaburi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Phichit') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Phitsanulok') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Phra Nakhon Si Ayutthaya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Phrae') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Prachin Buri') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Prachuap Khiri Khan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Ranong') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Ratchaburi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Rayong') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Roi Et') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Sa Kaeo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Sakon Nakhon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Samut Prakan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Samut Sakhon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Samut Songkhram') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Sara Buri') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Satun') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Sing Buri') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Sisaket') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Songkhla') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Sukhothai') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Suphan Buri') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Surat Thani') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Surin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Tak') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Trang') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Trat') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Ubon Ratchathani') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Udon Thani') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Uthai Thani') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Uttaradit') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Yala') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Changwat Yasothon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Chiang Mai Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Thailand', 'Phuket') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Togo', 'Maritime') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Togo', 'Savanes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tokelau', 'Atafu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tonga', 'Tongatapu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tonga', 'Vava`u') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Trinidad and Tobago', 'Borough of Arima') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Trinidad and Tobago', 'Chaguanas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Trinidad and Tobago', 'City of Port of Spain') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Trinidad and Tobago', 'City of San Fernando') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Trinidad and Tobago', 'Couva-Tabaquite-Talparo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Trinidad and Tobago', 'Diego Martin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Trinidad and Tobago', 'Eastern Tobago') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Trinidad and Tobago', 'Mayaro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Trinidad and Tobago', 'Penal/Debe') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Trinidad and Tobago', 'Princes Town') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Trinidad and Tobago', 'San Juan/Laventille') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Trinidad and Tobago', 'Sangre Grande') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Trinidad and Tobago', 'Siparia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Trinidad and Tobago', 'Tobago') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Trinidad and Tobago', 'Tunapuna/Piarco') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tunisia', 'Gafsa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tunisia', 'Gouvernorat de Beja') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tunisia', 'Gouvernorat de l''Ariana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tunisia', 'Gouvernorat de Monastir') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tunisia', 'Gouvernorat de Nabeul') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tunisia', 'Gouvernorat de Sfax') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tunisia', 'Gouvernorat de Sidi Bouzid') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tunisia', 'Gouvernorat de Sousse') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tunisia', 'Gouvernorat de Tunis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tunisia', 'Manouba') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Adana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Adiyaman') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Afyonkarahisar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Agri') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Aksaray') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Amasya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Ankara') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Antalya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Ardahan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Artvin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Aydin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Balikesir') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Bartin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Batman') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Bayburt') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Bilecik') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Bingöl') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Bitlis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Bolu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Burdur') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Bursa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Çanakkale') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Çankiri') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Çorum') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Denizli') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Diyarbakir') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Duezce') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Edirne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Elazig') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Erzincan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Erzurum') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Eskisehir') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Gaziantep') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Giresun') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Guemueshane') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Hakkâri') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Hatay') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Igdir') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Isparta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Istanbul') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Izmir') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Kahramanmaras') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Karabuek') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Karaman') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Kars') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Kastamonu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Kayseri') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Kilis') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Kirikkale') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Kirklareli') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Kirsehir') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Kocaeli') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Konya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Kütahya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Malatya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Manisa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Mardin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Mersin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Mugla') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Mus') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Nevsehir') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Nigde') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Ordu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Osmaniye') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Rize') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Sakarya') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Samsun') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Sanliurfa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Siirt') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Sinop') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Sirnak') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Sivas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Tekirdag') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Tokat') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Trabzon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Tunceli') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Usak') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Van') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Yalova') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Yozgat') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkey', 'Zonguldak') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Turkmenistan', 'Ahal') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Tuvalu', 'Funafuti') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('U.S. Virgin Islands', 'Saint Croix Island') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('U.S. Virgin Islands', 'Saint John Island') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('U.S. Virgin Islands', 'Saint Thomas Island') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Uganda', 'Kampala District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Cherkas''ka Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Chernihiv') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Chernivtsi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Crimea') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Dnipropetrovska Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Donets''ka Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Ivano-Frankivs''ka Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Kharkivs''ka Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Khersons''ka Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Khmel''nyts''ka Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Kirovohrads''ka Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Kyiv City') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Kyiv Oblast') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Luhans''ka Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'L''vivs''ka Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Misto Sevastopol''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Mykolayivs''ka Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Odessa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Poltavs''ka Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Rivnens''ka Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Sums''ka Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Ternopil''s''ka Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Vinnyts''ka Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Volyns''ka Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Zakarpattia Oblast') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Zaporizhia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Ukraine', 'Zhytomyrs''ka Oblast''') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Arab Emirates', 'Abu Dhabi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Arab Emirates', 'Al Fujayrah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Arab Emirates', 'Ash Shariqah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Arab Emirates', 'Dubai') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Arab Emirates', 'Ra''s al Khaymah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Aberdeen City') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Aberdeenshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Anglesey') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Angus') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Antrim') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Ards District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Argyll and Bute') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Armagh District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Ballymena District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Ballymoney District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Banbridge District') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Barnsley') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Bath and North East Somerset') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Bedford') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Belfast') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Birmingham') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Blackburn with Darwen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Blackpool') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Bolton') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Borough of Bury') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Bournemouth') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Bracknell Forest') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Bradford') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Brighton and Hove') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Bristol') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Buckinghamshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Caernarfonshire and Merionethshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Caerphilly County Borough') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Calderdale') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Cambridgeshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Cardiff') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Carmarthenshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Carrickfergus') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Castlereagh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Central Bedfordshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Ceredigion') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Cheshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Cheshire East') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'City and County of Swansea') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'City of London') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'City of Newport') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Clackmannanshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Coleraine') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Cookstown') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Cornwall') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'County Borough of Blaenau Gwent') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'County Borough of Bridgend') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'County Borough of Conwy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'County Borough of Neath Port Talbot') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'County Borough of Rhondda Cynon Taf') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Coventry') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Craigavon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Cumbria') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Darlington') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Denbighshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Derby') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Derbyshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Devon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Doncaster') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Dorset') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Down') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Dudley') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Dumfries and Galloway') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Dundee City') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Dungannon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Durham') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'East Ayrshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'East Dunbartonshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'East Lothian') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'East Renfrewshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'East Riding of Yorkshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'East Sussex') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Edinburgh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'England') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Essex') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Falkirk') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Fermanagh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Fife') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Flintshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Gateshead') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Glasgow City') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Gloucestershire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Halton') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Hampshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Hartlepool') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Herefordshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Hertfordshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Highland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Inverclyde') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Isle of Wight') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Kent') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Kingston upon Hull') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Kirklees') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Knowsley') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Lancashire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Larne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Leeds') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Leicester') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Leicestershire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Limavady') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Lincolnshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Lisburn') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Liverpool') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Londonderry') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Luton') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Magherafelt') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Manchester') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Medway') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Merthyr Tydfil County Borough') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Middlesbrough') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Midlothian') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Milton Keynes') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Monmouthshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Moray') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Moyle') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Newcastle upon Tyne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Newry and Mourne') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Newtownabbey') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Norfolk') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'North Ayrshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'North Down') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'North East Lincolnshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'North Lanarkshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'North Lincolnshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'North Somerset') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'North Tyneside') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'North Yorkshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Northamptonshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Northern Ireland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Northumberland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Nottingham') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Nottinghamshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Oldham') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Omagh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Orkney Islands') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Oxfordshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Pembrokeshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Perth and Kinross') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Peterborough') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Plymouth') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Poole') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Portsmouth') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Powys') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Reading') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Redcar and Cleveland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Renfrewshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Rochdale') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Rotherham') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Rutland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Salford') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Sandwell') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Sefton') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Sheffield') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Shetland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Shropshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Slough') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Solihull') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Somerset') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'South Ayrshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'South Gloucestershire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'South Lanarkshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'South Tyneside') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Southampton') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Southend-on-Sea') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'St. Helens') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Staffordshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Stirling') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Stockport') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Stockton-on-Tees') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Stoke-on-Trent') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Strabane') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Suffolk') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Sunderland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Surrey') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Swindon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Tameside') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Telford and Wrekin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'The Scottish Borders') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Thurrock') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Torbay') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Torfaen County Borough') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Trafford') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Vale of Glamorgan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Wakefield') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Walsall') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Warrington') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Warwickshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'West Berkshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'West Dunbartonshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'West Lothian') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'West Sussex') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Western Isles') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Wigan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Wiltshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Windsor and Maidenhead') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Wirral') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Wokingham') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Wolverhampton') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Worcestershire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'Wrexham') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United Kingdom', 'York') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Alabama') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Alaska') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Arizona') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Arkansas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'California') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Colorado') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Connecticut') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Delaware') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'District of Columbia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Florida') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Georgia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Hawaii') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Idaho') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Illinois') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Indiana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Iowa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Kansas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Kentucky') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Louisiana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Maine') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Maryland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Massachusetts') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Michigan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Minnesota') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Mississippi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Missouri') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Montana') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Nebraska') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Nevada') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'New Hampshire') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'New Jersey') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'New Mexico') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'New York') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'North Carolina') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'North Dakota') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Ohio') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Oklahoma') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Oregon') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Pennsylvania') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Rhode Island') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'South Carolina') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'South Dakota') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Tennessee') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Texas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Utah') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Vermont') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Virginia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Washington') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'West Virginia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Wisconsin') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('United States', 'Wyoming') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Uruguay', 'Canelones') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Uruguay', 'Colonia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Uruguay', 'Departamento de Durazno') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Uruguay', 'Departamento de Montevideo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Uruguay', 'Departamento de Paysandu') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Uruguay', 'Departamento de Salto') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Uruguay', 'Departamento de Tacuarembo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Uruguay', 'Florida') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Uruguay', 'Maldonado') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Uruguay', 'Soriano') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Uzbekistan', 'Qashqadaryo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Uzbekistan', 'Samarqand Viloyati') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Uzbekistan', 'Surxondaryo Viloyati') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Uzbekistan', 'Toshkent Shahri') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vanuatu', 'Penama Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vanuatu', 'Shefa Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Amazonas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Anzoátegui') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Apure') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Aragua') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Barinas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Bolívar') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Capital') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Carabobo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Delta Amacuro') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Dependencias Federales') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Estado Trujillo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Falcón') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Guárico') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Lara') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Mérida') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Miranda') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Monagas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Nueva Esparta') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Portuguesa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Sucre') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Táchira') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Vargas') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Yaracuy') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Venezuela', 'Zulia') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'An Giang') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Gia Lai') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Hau Giang') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Ho Chi Minh City') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Huyen Dien Bien') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Kon Tum') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Thanh Pho Can Tho') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Thanh Pho Da Nang') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Thanh Pho Ha Noi') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Thanh Pho Hai Phong') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Ba Ria-Vung Tau') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Bac Giang') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Bac Kan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Bac Ninh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Ben Tre') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Binh Dinh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Binh Duong') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Binh Thuan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Ca Mau') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Dak Lak') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Dong Nai') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Ha Giang') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Ha Tinh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Hai Duong') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Hoa Binh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Hung Yen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Khanh Hoa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Lai Chau') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Lam Dong') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Lang Son') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Lao Cai') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Nam Dinh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Nghe An') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Ninh Binh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Ninh Thuan') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Phu Tho') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Quang Nam') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Quang Ngai') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Quang Ninh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Quang Tri') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Soc Trang') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Son La') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Tay Ninh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Thai Binh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Thai Nguyen') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Thanh Hoa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Thua Thien-Hue') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Tien Giang') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Tra Vinh') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Tuyen Quang') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Vinh Long') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Vinh Phuc') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Vietnam', 'Tinh Yen Bai') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Wallis and Futuna', 'Circonscription d''Uvea') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Yemen', 'Aden') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Yemen', 'Muhafazat Hadramawt') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Yemen', 'Sanaa') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Zambia', 'Central Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Zambia', 'Copperbelt') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Zambia', 'Lusaka Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Zambia', 'North-Western Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Zambia', 'Southern Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Zimbabwe', 'Bulawayo') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Zimbabwe', 'Harare') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Zimbabwe', 'Manicaland') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Zimbabwe', 'Mashonaland East Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Zimbabwe', 'Mashonaland West') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Zimbabwe', 'Matabeleland North') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Zimbabwe', 'Matabeleland South Province') 

INSERT INTO [VitalSigns].[dbo].[ValidLocations] ([Country], [State]) VALUES  ('Zimbabwe', 'Midlands Province') 

END