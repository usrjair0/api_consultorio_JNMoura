create database consultorio2
use consultorio2;
--sp_help medicamento;
--sp_help medico;
--sp_help paciente;
create table medico (
	id int identity(1,1),
	crm varchar(9),
	nome varchar(100) not null,
	constraint PK_medico primary key (crm)
);

create table Paciente (
	id int identity(1,1),
	nome varchar(100) not null,
	datanascimento date not null,
	constraint PK_paciente primary key (id)
);

create table medicamento(
	id int identity(1,1),
	nome varchar(100) not null,
	datafabricacao date not null,
	datavencimento date,
	constraint PK_medicamento primary key (id)
);



select id, nome, datafabricacao, datavencimento from medicamento;
insert into medicamento (nome, datafabricacao, datavencimento) values ('Dipirona','1995-08-20', null);
insert into medicamento (nome, datafabricacao, datavencimento) values ('Buscopan','1999-04-05', '2000-04-05');
insert into medicamento (nome, datafabricacao, datavencimento) values ('Vitamina C', '2005-07-19', '2010-06-06');

select id, crm, nome from medico;
insert into medico (nome, crm) values ('João Gomes','123456/SP');
insert into medico (nome, crm) values ('Jairo Silva','123457/SP');
insert into medico (nome, crm) values ('Leticia Albuquerque','123458/SP');
insert into medico (nome, crm) values ('Thiago José','123459/RN');


select convert(int, SCOPE_IDENTITY());

select id, nome, datanascimento from Paciente;
insert into Paciente (nome, datanascimento) values ('joão Silva','1999-07-04');
insert into Paciente (nome, datanascimento) values ('José Firmino','2002-07-04');
insert into Paciente (nome, datanascimento) values ('Abner Silva','1970-09-10');
insert into Paciente (nome, datanascimento) values ('Clotilde Albuquerque','1989-10-25');

