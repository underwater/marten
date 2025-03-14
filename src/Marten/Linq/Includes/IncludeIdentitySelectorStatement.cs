using System;
using System.Collections.Generic;
using System.Linq;
using Marten.Internal;
using Marten.Linq.QueryHandlers;
using Marten.Linq.Selectors;
using Marten.Linq.SqlGeneration;
using Weasel.Postgresql;
using StringExtensions = JasperFx.Core.StringExtensions;

namespace Marten.Linq.Includes;

// This is also used as an ISelectClause inside the statement structure
internal class IncludeIdentitySelectorStatement: Statement, ISelectClause
{
    private readonly SelectorStatement _clonedEnd;
    private readonly IList<IIncludePlan> _includes;
    private readonly SelectorStatement _innerEnd;

    public IncludeIdentitySelectorStatement(Statement original, IList<IIncludePlan> includes,
        IMartenSession session): base(null)
    {
        ExportName = session.NextTempTableName();

        OriginalPaging = new PagedStatement(original);

        _includes = includes;

        _innerEnd = (SelectorStatement)original.Current();
        FromObject = _innerEnd.SelectClause.FromObject;

        _clonedEnd = _innerEnd.UseAsEndOfTempTableAndClone(this);
        _clonedEnd.SingleValue = _innerEnd.SingleValue;

        Inner = original;
        Inner.Limit = 0; // Watch this!
        Inner.Offset = 0;

        Statement current = this;
        foreach (var include in includes)
        {
            var includeStatement = include.BuildStatement(ExportName, OriginalPaging);

            current.InsertAfter(includeStatement);
            current = includeStatement;
        }

        current.InsertAfter(_clonedEnd);
    }

    public IPagedStatement OriginalPaging { get; }

    public Statement Inner { get; private set; }

    public bool IncludeDataInTempTable { get; set; }

    public Type SelectedType => typeof(void);

    public void WriteSelectClause(CommandBuilder sql)
    {
        if (IncludeDataInTempTable)
        {
            // Basically if the data for the Include is coming from a
            // SelectMany() clause
            sql.Append("select data, ");
        }
        else
        {
            sql.Append("select id, ");
        }

        sql.Append(StringExtensions.Join(_includes.Select(x => x.TempTableSelector), ", "));
        sql.Append(" from ");
        sql.Append(FromObject);
        sql.Append(" as d ");
    }

    public string[] SelectFields()
    {
        return _includes.Select(x => x.TempTableSelector).ToArray();
    }

    public ISelector BuildSelector(IMartenSession session)
    {
        throw new NotSupportedException();
    }

    public IQueryHandler<T> BuildHandler<T>(IMartenSession session, Statement topStatement,
        Statement currentStatement)
    {
        // It's wrapped in LinqHandlerBuilder
        return _clonedEnd.SelectClause.BuildHandler<T>(session, topStatement, currentStatement);
    }

    public ISelectClause UseStatistics(QueryStatistics statistics)
    {
        throw new NotSupportedException();
    }


    public override void CompileLocal(IMartenSession session)
    {
        Inner.CompileStructure(session);
        Inner = Inner.Top();
    }

    protected override void configure(CommandBuilder sql)
    {
        sql.Append("drop table if exists ");
        sql.Append(ExportName);
        sql.Append(";\n");
        sql.Append("create temp table ");
        sql.Append(ExportName);
        sql.Append(" as (\n");
        Inner.Configure(sql);
        sql.Append("\n);");
    }
}
