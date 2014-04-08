if(select databaseproperty('dnt','isfulltextenabled'))=0 
  execute sp_fulltext_database 'enable'
;
if exists (select * from sysfulltextcatalogs where name ='pk_dnt_posts1_msg')
   execute sp_fulltext_catalog 'pk_dnt_posts1_msg','drop'
;

if exists (select * from sysfulltextcatalogs where name ='pk_dnt_posts1_msg')
   execute sp_fulltext_table '[dnt_posts1]', 'drop' 
;


execute sp_fulltext_catalog 'pk_dnt_posts1_msg','create';



execute sp_fulltext_table '[dnt_posts1]','create','pk_dnt_posts1_msg','pk_dnt_posts1'
;


execute sp_fulltext_column '[dnt_posts1]','message','add'
;

execute sp_fulltext_table '[dnt_posts1]','activate'
;

execute sp_fulltext_catalog 'pk_dnt_posts1_msg','start_full'
;
