use asf_cms;

update publication set status=1 where datediff(unpublished,curdate()) >0;
update publication set status=3 where datediff(unpublished,curdate()) <=0;
update publication set status=2 where datediff(published,curdate()) >0;
update audit_report, publication set audit_report.published=case when publication.status=1 then 1 else 0 end where permalink=publication_permalink;
